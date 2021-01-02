module Handler exposing (..)

import Json.Decode as D
import Json.Encode as E


type alias Input =
    Int


type alias Output =
    Int


handle : Input -> Result {errorMessage:String,errorCode:Int} Output
handle input =
    Ok input


decoder : D.Decoder Input
decoder =
    let
        convertToInput : String -> Maybe Input
        convertToInput str =
            String.toInt str

        mapResult inputString =
            case inputString of
                Just result ->
                    D.succeed result

                Nothing ->
                    D.fail "excpected Int"
    in
    D.string
        |> D.map convertToInput
        |> D.andThen mapResult


encoder : Output -> E.Value
encoder =
    E.int
