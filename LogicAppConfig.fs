module TestFarmer1


type Connection =
    { connectionId: string
      connectionName: string
      id: string }

type LogicAppConfiguration =
    { connections: (string * Connection) []
      environment: string
      tenant:string
      clientId:string
      secrete:string
      resourceGroupName: string }

let dynamicsAxUAT =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-UAT/providers/Microsoft.Web/connections/dynamicsax"
      connectionName = "dynamicsax"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/providers/Microsoft.Web/locations/eastus/managedApis/dynamicsax" }


let serviceBusUAT =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-UAT/providers/Microsoft.Web/connections/servicebus-1"
      connectionName = "servicebus-1"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/providers/Microsoft.Web/locations/eastus/managedApis/servicebus" }



let DynataCustomConnectorUAT =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-UAT/providers/Microsoft.Web/connections/DynataCustomConnector-UAT-1"
      connectionName = "DynataCustomConnector-UAT-1"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-UAT/providers/Microsoft.Web/customApis/DynataCustomConnector-UAT" }


let destinationLogicAppConfigUAT =
    { connections =
          [| ("dynamicsax", dynamicsAxUAT)
             ("servicebus", serviceBusUAT)
             ("DynataCustomConnector", DynataCustomConnectorUAT) |]
      environment = ""
      clientId = ""
      tenant = ""
      secrete = ""
      resourceGroupName = "" }
    


