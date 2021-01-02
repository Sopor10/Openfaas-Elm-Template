port module Main exposing (main)
 
import Handler
import Json.Decode as D
import Json.Encode as E
import Platform exposing (worker)


main : Program () () Msg
main =
    worker
        { init = always ( (), Cmd.none )
        , update = update
        , subscriptions = always (start Start)
        }


update : Msg -> () -> ( (), Cmd Msg )
update (Start value) _ =
    ( (), initiate value )


type Msg
    = Start D.Value


type alias JobId =
    String


type Job a
    = Job JobId a


type alias JobResult =
    Result ( JobId, String ) (Job Output)


type Input
    = F1Input Handler.Input


type Output
    = F1Output Handler.Output


run : Job Input -> JobResult
run (Job jobId input) =
    let
        go run_ input_ outputConstructor =
            run_ input_
                |> Result.map outputConstructor
                |> Result.map (Job jobId)
                |> Result.mapError (\error -> ( jobId, error ))
    in
    case input of
        F1Input input_ ->
            go Handler.handle input_ F1Output




decodeInput : String -> D.Decoder Input
decodeInput functionId =
    let
        go inputDecoder inputConstructor =
            D.field "input" inputDecoder
                |> D.map inputConstructor
    in
    case functionId of
        "f1" ->
            go Handler.decoder F1Input

        _ ->
            D.fail "function not supported"


encodeOutput : JobResult -> E.Value
encodeOutput result =
    let
        go jobId outputEncoder output_ =
            E.object
                [ ( "status", E.string "ok" )
                , ( "jobId", E.string jobId )
                , ( "output", outputEncoder output_ )
                ]
    in
    case result of
        Ok (Job jobId out) ->
            case out of
                F1Output output_ ->
                    go jobId Handler.encoder output_

        Err ( jobId, error ) ->
            E.object
                [ ( "status", E.string "error" )
                , ( "jobId", E.string jobId )
                , ( "msg", E.string error )
                ]


initiate : E.Value -> Cmd Msg
initiate value =
    case D.decodeValue decodeJobId value of
        Err e ->
            output
                (E.object
                    [ ( "status", E.string "error" )
                    , ( "msg", E.string (D.errorToString e) )
                    ]
                )

        Ok jobId ->
            case D.decodeValue decoder value of
                Err e ->
                    output
                        (E.object
                            [ ( "status", E.string "error" )
                            , ( "jobId", E.string jobId )
                            , ( "msg", E.string (D.errorToString e) )
                            ]
                        )

                Ok job ->
                    job
                        |> run
                        |> encodeOutput
                        |> output


decodeJobId : D.Decoder String
decodeJobId =
    D.field "jobId" D.string
        |> D.andThen
            (\jobId ->
                if String.length jobId == 32 then
                    D.succeed jobId

                else
                    D.fail "invalid jobId length; only 32 chars allowed"
            )


decoder : D.Decoder (Job Input)
decoder =
    D.map2 Job
        decodeJobId
        (D.field "functionId" D.string
            |> D.andThen decodeInput
        )


port start : (D.Value -> msg) -> Sub msg


port output : E.Value -> Cmd msg
