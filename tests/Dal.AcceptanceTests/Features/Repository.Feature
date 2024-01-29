Feature: Repository

Scenario: CRUD Operations with the repository

Given the currency repository with the connection to the <RdbmsType> <DbName> database
When I use CreateAsync to create currency with <Id>, <Code>, <Name>, <Territory>
Then I can use GetAsync to fetch the currency with the <Id>

Examples:
    | RdbmsType | DbName      |Id | Code | Name            | Territory     |
    | pgsql     | testbankdb  |1  | USD  | US Dollar       | United States |
    | pgsql     | testbankdb  |2  | CAD  | Canadian Dollar | Canada        |
    | pgsql     | testbankdb  |3  | NGN  | Nigerian Naira  | Nigeria       |


