module BuildTemplateParameters

open System
open Microsoft.FSharp.Collections
open System.Text.Json

let addParam key value typeValue=
    (sprintf
        """
					 "%s":{
					 "defaultValue": "%s",
					  "type": "%s"
					 }
					 """
        key
        value
        typeValue)

let addParamObject key =
    (sprintf
        """
                         "%s":{
                         "defaultValue": {},
                          "type": "Object"
                         }
                         """
        key)

let BuildValueObject value = (sprintf """"value":%s""" value)
let BuildConnectionObject key value = (sprintf """"%s":%s""" key value)

let returnConnectionParamValue (logicAppConfigration: (string * TestFarmer1.Connection)) =
    match logicAppConfigration with
    | (key, connection) when key.Contains("dynamics") ->
        BuildConnectionObject key (JsonSerializer.Serialize(connection, JsonSerializerOptions(WriteIndented = true)))
    | (key, connection) when key.Contains("servicebus") ->
        BuildConnectionObject key (JsonSerializer.Serialize(connection, JsonSerializerOptions(WriteIndented = true)))
    | (key, connection) when key.Contains("DynataCustomConnector") ->
        BuildConnectionObject key (JsonSerializer.Serialize(connection, JsonSerializerOptions(WriteIndented = true)))
    | _ -> ""

let returnParamValue (logicAppConfigration: TestFarmer1.LogicAppConfigration) paramName =
    match paramName with
    | (paramName: string) when paramName.Contains("Environment") -> addParam paramName logicAppConfigration.environment "String"
    | (paramName: string) when paramName.Contains("ClientId") -> addParam paramName logicAppConfigration.clientId "String"
    | (paramName: string) when paramName.Contains("Secret") -> addParam paramName logicAppConfigration.secrete "SecureString"
    | (paramName: string) when paramName.Contains("Tenant") -> addParam paramName logicAppConfigration.tenant "String"
    | paramName when paramName.Contains("$connections") -> addParamObject paramName
    | _ -> ""

let buildParametersAndStoreInArray logicAppConfigration (paramsNames: string []) =
    paramsNames
    |> Array.map (returnParamValue logicAppConfigration)

let addNewParameterToArray newParameter (parameters: string []) =
    [| newParameter |] |> Array.append parameters

let convertParametersToJsonString (parameters: string []) =
    let temp =
        parameters
        |> Array.filter (fun x -> String.IsNullOrEmpty(x) = false)
        |> String.concat ","

    "{" + temp + "}"
