Feature: Repository

Background: CreateAsync with Customer and Account Repositories
    Given the testbankdb database is hosted on a pgsql rdbms

    And the following customers

    | Id     | FirstName | LastName | MiddleName | DateOfBirth |
    | 12345	 | Emily	 | Johnson	| Marie	     | 1985-07-12  |
    | 67890	 | Alexander | Rodriguez| Luis	     | 1992-04-25  |
    | 13579	 | Samantha	 | Davis	| Grace	     | 1988-11-03  |

    And their corresponding accounts
    | Id     | Name          | Description     | CurrencyId | AccountTypeId | CustomerId | Balance |
    | 101001 | Emily Johnson | Savings account | 3          | 1             | 12345      | 0       |
    | 201005 | Emily Johnson | Current account | 3          | 3             | 12345      | 0       |
    | 201006 | Alex R        | Current account | 3          | 3             | 67890      | 0       |
    | 201008 | Grace         | Current account | 3          | 3             | 13579      | 0       |
    | 201010 | Grace         | Current account | 1          | 3             | 13579      | 0       |

Scenario: CRUD Operations with the repository

Given the currency repository with the connection to the <RdbmsType> <DbName> database
When I use CreateAsync to create currency with <Id>, <Code>, <Name>, <Territory>
Then I can use GetAsync to fetch the currency with the <Id>
Examples:
    | RdbmsType | DbName      |Id | Code | Name            | Territory     |
    | pgsql     | testbankdb  |10 | NGN  | Nigerian Naira | Nigeria        |




