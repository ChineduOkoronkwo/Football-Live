------------------------------------------------
------ Tables For Bank Database ----------------
------------------------------------------------

-- sudo -u postgres psql
-- postgres=# create database @dbname;
-- postgres=# create user @dbuser with encrypted password @dbpsswd;
-- postgres=# grant all privileges on database mydb to myuser;

-- create database testbankdb;
-- create user @dbuser with password @dbpsswd;
-- grant all on database testbankdb to @dbuser;
-- GRANT ALL ON ALL TABLES IN SCHEMA "public" TO @dbuser;

CREATE TABLE Customer (
    Id INTEGER PRIMARY KEY,
    FirstName VARCHAR(30) NOT NULL,
    LastName VARCHAR(30) NOT NULL,
    MiddleName VARCHAR(30) NOT NULL,
    DateOfBirth DATE NOT NULL
);

CREATE TABLE Currency (
    Id INTEGER PRIMARY KEY,
    CODE CHAR(3) NOT NULL,
    Territory VARCHAR(40) NOT NULL,
    Name VARCHAR(30) NOT NULL
);

CREATE TABLE AccountType (
    Id INTEGER PRIMARY KEY,
    Name VARCHAR(20) NOT NULL,
    Description VARCHAR(50)
);

CREATE TABLE TransactionType (
    Id INTEGER PRIMARY KEY,
    Name VARCHAR(20) NOT NULL,
    Description VARCHAR(50)
);

CREATE TABLE Account (
    Id INTEGER PRIMARY KEY,
    Name VARCHAR(20),
    Description VARCHAR(50),
    CurrencyId INTEGER NOT NULL,
    AccountTypeId INTEGER NOT NULL,
    CustomerId INTEGER NOT NULL,
    Balance DECIMAL NOT NULL,
    FOREIGN KEY (CurrencyId) REFERENCES Currency(Id),
    FOREIGN KEY (AccountTypeId) REFERENCES AccountType(Id),
    FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
);

CREATE TABLE AccountTransaction (
    Id INTEGER PRIMARY KEY,
    AccountId INTEGER NOT NULL,
    Amt DECIMAL NOT NULL,
    CurrencyId INTEGER NOT NULL,
    TransactionTypeId INTEGER NOT NULL,
    Description VARCHAR(50),
    FOREIGN KEY (AccountId) REFERENCES Account(Id),
    FOREIGN KEY (CurrencyId) REFERENCES Currency(Id),
    FOREIGN KEY (TransactionTypeId) REFERENCES TransactionType(Id)
);

GRANT ALL ON ALL TABLES IN SCHEMA "public" TO @dbuser;
