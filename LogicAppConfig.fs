module TestFarmer1


type Connection =
    { connectionId: string
      connectionName: string
      id: string }

type LogicAppConfigration =
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

let dynamicsAxDev =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-DevTest/providers/Microsoft.Web/connections/dynamicsax"
      connectionName = "dynamicsax"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/providers/Microsoft.Web/locations/eastus/managedApis/dynamicsax" }

let servicebusUAT =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-UAT/providers/Microsoft.Web/connections/servicebus-1"
      connectionName = "servicebus-1"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/providers/Microsoft.Web/locations/eastus/managedApis/servicebus" }
let servicebusPROD =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-PROD/providers/Microsoft.Web/connections/servicebus"
      connectionName = "servicebus"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/providers/Microsoft.Web/locations/eastus/managedApis/servicebus" }


let DynataCustomConnectorDev =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-DevTest/providers/Microsoft.Web/connections/DynataCustomConnector-2"
      connectionName = "DynataCustomConnector-2"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-DevTest/providers/Microsoft.Web/customApis/DynataCustomConnector" }

let DynataCustomConnectorUAT =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-UAT/providers/Microsoft.Web/connections/DynataCustomConnector-UAT-1"
      connectionName = "DynataCustomConnector-UAT-1"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-UAT/providers/Microsoft.Web/customApis/DynataCustomConnector-UAT" }
let DynataCustomConnectorPROD =
    { connectionId =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-PROD/providers/Microsoft.Web/connections/DynataCustomConnector-PRD"
      connectionName = "DynataCustomConnector-PRD"
      id =
          "/subscriptions/d51bc4ba-b7b0-420a-a425-a8d166eb2f6b/resourceGroups/Dynata-Integrations-PROD/providers/Microsoft.Web/customApis/DynataCustomConnector-PRD" }


let logicAppConfigUAT =
    { connections =
          [| ("dynamicsax", dynamicsAxUAT)
             ("servicebus", servicebusUAT)
             ("DynataCustomConnector", DynataCustomConnectorUAT) |]
      environment = "dynata-uat.sandbox.operations.dynamics.com"
      clientId = "4f7144bd-c763-4592-9070-362b46259bd6"
      tenant = "f0ff917d-ab8c-4129-b13f-33be267a153b"
      secrete = "Zz17Q~XooIDGm43erz2krwoNUUVxdjJBRXHap"
      resourceGroupName = "Dynata-Integrations-UAT" }
    
let logicAppConfigPROD =
    { connections =
          [| ("dynamicsax", dynamicsAxUAT)
             ("servicebus", servicebusPROD)
             ("DynataCustomConnector", DynataCustomConnectorPROD) |]
      environment = "dynata-uat.sandbox.operations.dynamics.com"
      clientId = "4f7144bd-c763-4592-9070-362b46259bd6"
      tenant = "f0ff917d-ab8c-4129-b13f-33be267a153b"
      secrete = "Zz17Q~XooIDGm43erz2krwoNUUVxdjJBRXHap"
      resourceGroupName = "Dynata-Integrations-PROD" }

let logicAppConfigDev =
    { connections =
          [| ("dynamicsax", dynamicsAxDev)
             ("servicebus", servicebusUAT)
             ("DynataCustomConnector", DynataCustomConnectorDev) |]
      environment = "dynata-devtest58bd6b26a6912bafaos.cloudax.dynamics.com"
      clientId = "4f7144bd-c763-4592-9070-362b46259bd6"
      tenant = "f0ff917d-ab8c-4129-b13f-33be267a153b"
      secrete = "Zz17Q~XooIDGm43erz2krwoNUUVxdjJBRXHap"
      resourceGroupName = "Dynata-Integrations-DevTest" }
