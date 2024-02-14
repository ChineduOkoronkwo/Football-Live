Feature: Repository

Background: Use CreateAsync to create customer and account records
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


Scenario: Use ListAsync to read paginated customer and account records
    Given The ListSqlCommand of CustomerEntitySql is set to order records by LastName, FirstName
    When I use ListAsync on customer repo to read 100 records starting from record 0
    Then I can verify that there are 3 customer records which matches the following

    | Id     | FirstName | LastName | MiddleName | DateOfBirth |
    | 12345	 | Emily	 | Johnson	| Marie	     | 1985-07-12  |
    | 67890	 | Alexander | Rodriguez| Luis	     | 1992-04-25  |
    | 13579	 | Samantha	 | Davis	| Grace	     | 1988-11-03  |

    When I use ListAsync on customer repo to read 2 records starting from record 0
    Then I can verify that there are 2 customer records which matches the following

    | Id     | FirstName | LastName | MiddleName | DateOfBirth |
    | 13579	 | Samantha	 | Davis	| Grace	     | 1988-11-03  |
    | 12345	 | Emily	 | Johnson	| Marie	     | 1985-07-12  |

    When I use ListAsync on customer repo to read 2 records starting from record 2
    Then I can verify that there are 1 customer records which matches the following

    | Id     | FirstName | LastName | MiddleName | DateOfBirth |
    | 67890	 | Alexander | Rodriguez| Luis	     | 1992-04-25  |

    Given The ListSqlCommand of AccountEntitySql is set to order records by CurrencyId, Name, AccountTypeId
    When I use ListAsync on account repo to read 100 records starting from record 0
    Then I can verify that there are 5 account records which matches the following

    | Id     | Name          | Description     | CurrencyId | AccountTypeId | CustomerId | Balance |
    | 101001 | Emily Johnson | Savings account | 3          | 1             | 12345      | 0       |
    | 201005 | Emily Johnson | Current account | 3          | 3             | 12345      | 0       |
    | 201006 | Alex R        | Current account | 3          | 3             | 67890      | 0       |
    | 201008 | Grace         | Current account | 3          | 3             | 13579      | 0       |
    | 201010 | Grace         | Current account | 1          | 3             | 13579      | 0       |

    When I use ListAsync on account repo to read 2 records starting from record 0
    Then I can verify that there are 2 account records which matches the following

    | Id     | Name          | Description     | CurrencyId | AccountTypeId | CustomerId | Balance |
    | 201006 | Alex R        | Current account | 3          | 3             | 67890      | 0       |
    | 201010 | Grace         | Current account | 1          | 3             | 13579      | 0       |

    When I use ListAsync on account repo to read 2 records starting from record 2
    Then I can verify that there are 2 account records which matches the following

    | Id     | Name          | Description     | CurrencyId | AccountTypeId | CustomerId | Balance |
    | 101001 | Emily Johnson | Savings account | 3          | 1             | 12345      | 0       |
    | 201005 | Emily Johnson | Current account | 3          | 3             | 12345      | 0       |

    When I use ListAsync on account repo to read 2 records starting from record 4
    Then I can verify that there are 1 account records which matches the following

    | Id     | Name          | Description     | CurrencyId | AccountTypeId | CustomerId | Balance |
    | 201008 | Grace         | Current account | 3          | 3             | 13579      | 0       |


Scenario: Use UpdateAsync to update customer and account record and GetAsync to verify the updates
    When I update the MiddleName to Marrine and FirstName to Whitney for Customer 12345 
    Then I can verify that the changes to customer 12345 are persisted
    | Id     | FirstName | LastName | MiddleName | DateOfBirth |
    | 12345	 | Whitney	 | Johnson	| Marrine	 | 1985-07-12  |


Scenario: Use DeleteAsync to delete account record
    When I delete account <Id> using DeleteAsync
    Then I can verify that account <Id> no longer exist
    Examples:
    | Id     |
    | 101001 |
    | 201006 |
    | 201008 |


Scenario: Deleting customer that has existing account
    When I delete customer <Id> that also has an account using DeleteAsync
    Then I can verify the customer deletion throws an exception with <Message>
    And The customer record still exist
    | Id     | FirstName | LastName | MiddleName | DateOfBirth |
    | 12345	 | Emily	 | Johnson	| Marie	     | 1985-07-12  |

    Examples:
    | Id    | Message                                                                                                                  |
    | 12345 | 23503: update or delete on table "customer" violates foreign key constraint "account_customerid_fkey" on table "account" |


Scenario: Deleting all customer accounts before deleting the customer
    When I delete account <AccountId> using DeleteAsync
    And I Delete the cutomer <CustomerId> that owns the account
    Then I can verify that account <AccountId> no longer exist
    And I can verify that customer  <CustomerId> no longer exist
    Examples:
    | AccountId | CustomerId |
    | 201006    | 67890      |


Scenario: Deleting existing customer accounts before deleting the customer in a transaction
    When I delete existing account <AccountId> and customer <CustomerId>
    Then I can verify that account <AccountId> no longer exist
    And I can verify that customer  <CustomerId> no longer exist
    Examples:
    | AccountId | CustomerId |
    | 201006    | 67890      |


Scenario: Deleting customer and one of its accounts in a transaction
    When I delete existing account <AccountId> and customer <CustomerId>
    Then I can verify that the account still exist
    | Id     | Name          | Description     | CurrencyId | AccountTypeId | CustomerId | Balance |
    | 201005 | Emily Johnson | Current account | 3          | 3             | 12345      | 0       |

    And The customer record still exist
    | Id     | FirstName | LastName | MiddleName | DateOfBirth |
    | 12345	 | Emily	 | Johnson	| Marie	     | 1985-07-12  |

    Examples:
    | AccountId | CustomerId | Message                                                                                                                  |
    | 201005    | 12345      | 23503: update or delete on table "customer" violates foreign key constraint "account_customerid_fkey" on table "account" |