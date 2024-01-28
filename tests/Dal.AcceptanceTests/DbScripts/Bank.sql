------------------------------------------------
------ Tables For Bank Database ----------------
------------------------------------------------

-- sudo -u postgres psql
-- postgres=# create database @dbname;
-- postgres=# create user @dbuser with encrypted password @dbpsswd;
-- postgres=# grant all privileges on database mydb to myuser;

create database @dbname;
create user @dbuser with encrypted password @dbpsswd;
grant all privileges on database @dbname to @dbuser;

CREATE TABLE Customer (
    Id UUID PRIMARY KEY,
    FirstName VARCHAR(30) NOT NULL,
    LastName VARCHAR(30) NOT NULL,
    MiddleName VARCHAR(30) NOT NULL,
    DateOfBirth DATE NOT NULL
);

CREATE TABLE Currency (
    CODE CHAR(3) PRIMARY KEY,
    Territory VARCHAR(40) NOT NULL,
    Name VARCHAR(30) NOT NULL
);

CREATE TABLE AccountType (
    Id UUID PRIMARY KEY,
    Name VARCHAR(20) NOT NULL,
    Description VARCHAR(50)
);

CREATE TABLE TransactionType (
    Id UUID PRIMARY KEY,
    Name VARCHAR(20) NOT NULL,
    Description VARCHAR(50)
);

CREATE TABLE Account (
    Id UUID PRIMARY KEY,
    Name VARCHAR(20),
    Description VARCHAR(50),
    CurrencyCode CHAR(3),
    AccountTypeId UUID NOT NULL,
    CustomerId UUID NOT NULL,
    Balance DECIMAL NOT NULL,
    FOREIGN KEY (CurrencyCode) REFERENCES Currency(Code),
    FOREIGN KEY (AccountTypeId) REFERENCES AccountType(Id)
    FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
);

CREATE TABLE AccountTransaction (
    Id UUID PRIMARY KEY,
    AccountId UUID NOT NULL,
    Amt DECIMAL NOT NULL,
    CurrencyCode CHAR(3) NOT NULL
    TransactionTypeId UUID NOT NULL,
    Description VARCHAR(50),
    FOREIGN KEY (AccountId) REFERENCES Account(Id),
    FOREIGN KEY (CurrencyCode) REFERENCES Currency(Code),
    FOREIGN KEY (TransactionTypeId) REFERENCES TransactionType(Id)
);
