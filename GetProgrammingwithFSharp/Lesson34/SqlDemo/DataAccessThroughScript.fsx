#r @"..\..\packages\SQLProvider\lib\FSharp.Data.SqlProvider.dll"
#r @"..\packages\FSharp.Data.SqlClient.2.0.2\lib\net40\FSharp.Data.SqlClient.dll"

#load "CustomerRepository.fs"

let scriptConnectionString = "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"

CustomerRepository.printCustomers(scriptConnectionString)
