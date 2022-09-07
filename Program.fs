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

    let MoveLogicApp logicAppName newLogicAppName (resourceGroupFrom:string) logicAppConfig =
        let call = runGetLogicAppTemplate resourceGroupFrom
        let temp1 =
            GetLogicAppId resourceGroupFrom logicAppName
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
            |> buildParametersAndStoreInArray logicAppConfig
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
                        logicAppConfig.connections
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
            addParam ("workflows_" + logicAppName + "_name") newLogicAppName "String"

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

        let fileName = $"{logicAppName}.json"
        File.WriteAllText(fileName, UpdatedTemplate.JsonValue.ToString())

        Az.az $"deployment group create --resource-group {logicAppConfig.resourceGroupName} --template-file {fileName}"
        |> printfn "%A"

//    MoveLogicApp "PushAccountToAX_Http_UAT" "PushAccountToAX_Http_PROD" "Dynata-Integrations-UAT" logicAppConfigPROD
//    MoveLogicApp "PushSalesOrderToAX_Http_UAT" "PushSalesOrderToAX_Http_PROD" "Dynata-Integrations-UAT" logicAppConfigPROD
//    MoveLogicApp "PushBillingSchedulingToAX_Http_UAT" "PushBillingSchedulingToAX_Http_PROD" "Dynata-Integrations-UAT" logicAppConfigPROD
//    MoveLogicApp "PushAccountToCRM_Http_UAT" "PushAccountToCRM_Http_PROD" "Dynata-Integrations-UAT" logicAppConfigPROD
//    MoveLogicApp "PushBstToCRM_Http_UAT" "PushBstToCRM_Http_PROD" "Dynata-Integrations-UAT" logicAppConfigPROD
//    MoveLogicApp "PushExchangeRate_UAT" "PushExchangeRate_PROD" "Dynata-Integrations-UAT" logicAppConfigPROD
//    MoveLogicApp "PushInvoiceToCRM_Http_UAT" "PushInvoiceToCRM_Http_PROD" "Dynata-Integrations-UAT" logicAppConfigPROD
//    MoveLogicApp  "PushVendorToCRM_Http_UAT" "PushVendorToCRM_Http_PROD"   "Dynata-Integrations-UAT" logicAppConfigPROD
//    MoveLogicApp "ExhangeRateFromAxToSBT_UAT" "ExhangeRateFromAxToSBT_PROD" "Dynata-Integrations-UAT" logicAppConfigPROD

    MoveLogicApp "PushAccountToAX_Http" "PushAccountToAX_Http_UAT" "Dynata-Integrations-DevTest" logicAppConfigUAT
    MoveLogicApp "PushSalesOrderToAX_Http" "PushSalesOrderToAX_Http_UAT" "Dynata-Integrations-DevTest" logicAppConfigUAT
    MoveLogicApp "PushBillingSchedulingToAX_Http" "PushBillingSchedulingToAX_Http_UAT" "Dynata-Integrations-DevTest" logicAppConfigUAT
    MoveLogicApp "PushAccountToCRM_Http" "PushAccountToCRM_Http_UAT" "Dynata-Integrations-DevTest" logicAppConfigUAT
    MoveLogicApp "PushBstToCRM_Http" "PushBstToCRM_Http_UAT" "Dynata-Integrations-DevTest" logicAppConfigUAT
    MoveLogicApp "PushExchangeRate" "PushExchangeRate_UAT" "Dynata-Integrations-DevTest" logicAppConfigUAT
    MoveLogicApp "PushInvoiceToCRM_Http" "PushInvoiceToCRM_Http_UAT" "Dynata-Integrations-DevTest" logicAppConfigUAT
    MoveLogicApp  "PushVendorToCRM_Http" "PushVendorToCRM_Http_UAT"   "Dynata-Integrations-DevTest"logicAppConfigUAT
    MoveLogicApp "ExhangeRateFromAxToSBT" "ExhangeRateFromAxToSBT_UAT" "Dynata-Integrations-DevTest" logicAppConfigUAT
    
    
//    MoveLogicApp  "PushAccountToAX_Http_UAT" "PushAccountToAX_Http"  "Dynata-Integrations-UAT" logicAppConfigDev
//    MoveLogicApp "PushSalesOrderToAX_Http_UAT" "PushSalesOrderToAX_Http"  "Dynata-Integrations-UAT" logicAppConfigDev
//    MoveLogicApp "PushBillingSchedulingToAX_Http_UAT" "PushBillingSchedulingToAX_Http"  "Dynata-Integrations-UAT" logicAppConfigDev
//    MoveLogicApp "PushAccountToCRM_UAT" "PushAccountToCRM" "Dynata-Integrations-UAT" logicAppConfigDev
//    MoveLogicApp "PushBstToCRM_UAT" "PushBstToCRM" "Dynata-Integrations-UAT" logicAppConfigDev
//    MoveLogicApp "PushExchangeRate_UAT" "PushExchangeRate" "Dynata-Integrations-UAT" logicAppConfigDev
//    MoveLogicApp "PushInvoiceToCRM_UAT" "PushInvoiceToCRM" "Dynata-Integrations-UAT" logicAppConfigDev
//    MoveLogicApp "PushVendorToCRM_UAT" "PushVendorToCRM" "Dynata-Integrations-UAT" logicAppConfigDev
//    MoveLogicApp "ExhangeRateFromAxToSBT_UAT" "ExhangeRateFromAxToSBT" "Dynata-Integrations-UAT" logicAppConfigDev
