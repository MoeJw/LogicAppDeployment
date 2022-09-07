namespace TestFarmer

open Farmer.Deploy
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Core
open FSharp.Data
open BuildTemplateParameters
open System.IO
open TestFarmer1
open System.Text.Json

module Say =
    let GetLogicAppId (resourceGroupName:string) logicAppName =
        $"""resource show --resource-group {resourceGroupName} --name {logicAppName} --resource-type Microsoft.Logic/workflows --query id"""
        |> Az.az

    let GetLogicAppTemplate resourceGroupFrom id =
        $"""group export --resource-group {resourceGroupFrom} --resource-ids {id}"""
        |> Az.az

    let validate (_result: Result<string, string>) doFunction =
        match _result with
        | Ok s -> doFunction s
        | Error e -> Error e
    let runGetLogicAppTemplate resourceGroupFrom x = validate x (GetLogicAppTemplate resourceGroupFrom)
    type Simple = JsonProvider<"./LogicAppSample.json">

    let MoveLogicApp sourceLogicAppName newLogicAppName (sourceResourceGroup:string) destinationLogicAppConfig =
        let call = runGetLogicAppTemplate sourceResourceGroup
        let temp1 =
            GetLogicAppId sourceResourceGroup sourceLogicAppName
            |> call

        let result =
            match temp1 with
            | Ok s -> s
            | Error e -> e


        let simple = Simple.Parse(result)
        let resource = (simple.Resources |> Array.get) 0
        let proper = resource.Properties
        let def = proper.Definition

        let logicAppParam =
            def.Parameters.JsonValue.Properties()
            |> Array.map (fun (x, a) -> x)
            |> buildParametersAndStoreInArray destinationLogicAppConfig
            |> convertParametersToJsonString
            |> JsonValue.Parse
            |> Simple.Parameters

        let Definition =
            Simple.Definition(def.Schema, def.ContentVersion, logicAppParam, def.Triggers, def.Actions, def.Outputs)

        let connectionsParameters =
            resource.Properties.Parameters.Connections.Value.JsonValue.Properties()
            |> Array.map
                (fun (stringValue, jsonValue) ->
                    let tr =
                        destinationLogicAppConfig.connections
                        |> Array.tryFind (fun (a, b) -> stringValue.Contains(a))

                    match tr with
                    | Some (connectionName, connectionValue) -> Some(stringValue, connectionValue)
                    | _ -> None)
            |> Array.filter (fun (value) -> value.IsSome)
            |> Array.map (fun (value) -> value.Value)
            |> Array.map returnConnectionParamValue
            |> convertParametersToJsonString
            |> BuildValueObject


        let resourceParameterConnection =
            Simple.Connections(JsonValue.Parse("{" + connectionsParameters + "}"))

        let resourceParameter =
            Simple.Parameters2(resourceParameterConnection)

        let properties =
            Simple.Properties(Definition, resourceParameter)

        let resource =
            Simple.Resourcis(resource.Name, resource.Type, resource.ApiVersion, resource.Location.JsonValue, properties)

        let workflowParam =
            addParam ("workflows_" + sourceLogicAppName + "_name") newLogicAppName "String"

        let Param =
            addNewParameterToArray workflowParam [||]
            |> convertParametersToJsonString

        let pram =
            Simple.Parameters(JsonValue.Parse(Param))

        let UpdatedTemplate =
            Simple.Root(
                simple.Schema,
                simple.ContentVersion,
                pram,
                [||],
                simple.Variables,
                [| resource |],
                simple.Variables
            )

        let fileName = $"{sourceLogicAppName}.json"
        File.WriteAllText(fileName, UpdatedTemplate.JsonValue.ToString())

        Az.az $"deployment group create --resource-group {destinationLogicAppConfig.resourceGroupName} --template-file {fileName}"
        |> printfn "%A"



    
    MoveLogicApp "source name logic app" "dis name logic app" "the source Name of the Resource group" destinationLogicAppConfigUAT
   
    
  
