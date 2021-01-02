'use strict'
const { Console } = require("console");
const { randomBytes } = require("crypto");
const { Elm } = require("./function/elm-main");
module.exports = async (event, context) => {
  const result = {
    
    'status':  await run("f1",event.body)
  }

  return context
    .status(200)
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
            resolve({ ok: v.output });
          } else if (v.status === "error") {
            resolve({ error: v.msg });
          } else {
            resolve({ error: "invalid response status" });
          }
        }
      };

      output.subscribe(go);

      timeout = setTimeout(() => {
        output.unsubscribe(go);
        resolve({ error: "invalid jobId or time limit exceeded" });
      }, 20 * 1000);
    } catch (e) {
      resolve({ error: "unexpected error" });
    }
  });

  setTimeout(() => {
    start.send({ jobId, functionId, input });
  }, Math.random() * 1000);

  return p;
};

