module Operations

open Domain

let deposit amount account : Account =
    { account with Balance = account.Balance + amount }

let withdraw amount account : Account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

let auditAs (operationName:string) (audit:Account -> string -> unit) (operation:decimal -> Account -> Account) (amount:decimal) (account:Account) : Account =
    let newAccount = operation amount account
    let message =
        if operationName = "deposit" then sprintf "Performed operation 'deposit' for $%s. Balance is now %s." (amount.ToString()) (newAccount.Balance.ToString())
        elif operationName = "withdraw" then sprintf "Performed operation 'withdraw' for $%s. Balance is now %s." (amount.ToString()) (newAccount.Balance.ToString())
        else failwith "Unknown operation"
    audit newAccount message
    newAccount

