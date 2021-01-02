'use strict'
const { Console } = require("console");
const { randomBytes } = require("crypto");
const { Elm } = require("./function/elm-main");
module.exports = async (event, context) => {
  const result = await run("f1",event.body);
  console.log(result);
  return context
    .status(result.statusCode)
    .succeed(result)
}


const App = Elm.Main.init({});

const run = async (functionId, input) => {
  const output = App.ports.output;
  const start = App.ports.start;

  const jobId = randomBytes(16).toString("hex");
  const p = new Promise(resolve => {
    try {
      let timeout;
      const go = v => {
        if (v.jobId === jobId) {
          clearTimeout(timeout);
          output.unsubscribe(go);
          console.log(input);

          if (v.status === "ok") {
            resolve({ value: v.output ,"statusCode" : v.statusCode});
          } else if (v.status === "error") {
            resolve({ value: v.msg, "statusCode" : v.statusCode });
          } else {
            resolve({ value: "invalid response status", "statusCode" : v.statusCode});
          }
        }
      };

      output.subscribe(go);

      timeout = setTimeout(() => {
        output.unsubscribe(go);
        resolve({ value: "invalid jobId or time limit exceeded" ,"statusCode" : 504});
      }, 20 * 1000);
    } catch (e) {
      resolve({ value: "unexpected error","statusCode" :500 });
    }
  });

  setTimeout(() => {
    start.send({ jobId, functionId, input });
  }, Math.random() * 1000);

  return p;
};

