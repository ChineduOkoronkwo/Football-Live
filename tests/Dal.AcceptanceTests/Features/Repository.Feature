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