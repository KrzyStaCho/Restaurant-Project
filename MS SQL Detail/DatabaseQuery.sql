/* --- Accounts Aspect ---

CREATE TABLE AccountGroup (
	GroupID INT IDENTITY(1,1) PRIMARY KEY,
	GroupName NVARCHAR(50) NOT NULL,
	IsAdmin BIT NOT NULL,
	CanChanged BIT DEFAULT 1 NOT NULL,
	WhoChanged NVARCHAR(50) NOT NULL,
	LastModified DATE DEFAULT GETDATE() NOT NULL)

CREATE TABLE Account (
	AccountID INT IDENTITY(1,1) PRIMARY KEY,
	Username NVARCHAR(50) NOT NULL,
	GroupID INT NOT NULL,
	LastOnline DATE DEFAULT NULL,
	CanChanged BIT DEFAULT 1 NOT NULL,
	WhoChanged NVARCHAR(50) NOT NULL,
	LastModified DATE DEFAULT GETDATE() NOT NULL,
	FOREIGN KEY(GroupID) REFERENCES AccountGroup(GroupID))

CREATE TABLE Password (
	AccountID INT NOT NULL PRIMARY KEY,
	Value NVARCHAR(MAX) NOT NULL,
	CanChanged BIT NOT NULL,
	WhoChanged NVARCHAR(50) NOT NULL,
	LastModified DATE DEFAULT GETDATE() NOT NULL,
	FOREIGN KEY(AccountID) REFERENCES Account(AccountID))

CREATE TABLE AccountArchive (
	Username NVARCHAR(50) NOT NULL,
	Password NVARCHAR(MAX) NOT NULL,
	GroupName NVARCHAR(50) NOT NULL,
	LastOnline DATE,
	WhoChanged NVARCHAR(50) NOT NULL,
	LastModified DATE NOT NULL,
	AccountID INT NOT NULL,
	ArchiveID INT IDENTITY(1,1) PRIMARY KEY)

CREATE TABLE Permission (
	PermissionID INT IDENTITY(1,1) PRIMARY KEY,
	Code VARCHAR(20) NOT NULL)

CREATE TABLE AccountGroupPerm (
	GroupID INT NOT NULL,
	PermissionID INT NOT NULL,
	PRIMARY KEY(GroupID, PermissionID),
	FOREIGN KEY(GroupID) REFERENCES AccountGroup(GroupID),
	FOREIGN KEY(PermissionID) REFERENCES Permission(PermissionID))

CREATE TRIGGER RemoveAccountTrigger ON Password
AFTER DELETE AS
	DECLARE @accountID INT
	SET @accountID = (SELECT AccountID FROM deleted)
	INSERT INTO AccountArchive (Username, Password, GroupName, LastOnline, WhoChanged, LastModified, AccountID)
	SELECT A.Username, P.Value, AG.GroupName, A.LastOnline, A.WhoChanged, GETDATE(), A.AccountID FROM Account AS A LEFT JOIN Password AS P ON A.AccountID = P.AccountID LEFT JOIN AccountGroup AS AG ON A.GroupID = AG.GroupID
	DELETE FROM Account WHERE AccountID = @accountID

INSERT INTO AccountGroup (GroupName, IsAdmin, CanChanged, WhoChanged) VALUES (N'System Admin', 1, 0, N'SysAdmin')
INSERT INTO Account (Username, GroupID, CanChanged, WhoChanged) VALUES (N'SysAdmin', 1, 0, N'SysAdmin')
INSERT INTO Password (AccountID, Value, CanChanged, WhoChanged) VALUES (1, N'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 1, N'SysAdmin')
*/

/* Supplier Aspect

CREATE TABLE Supplier (
	SupplierID INT IDENTITY(1,1) PRIMARY KEY,
	CompanyName NVARCHAR(50) NOT NULL,
	CompanyNIP NVARCHAR(10) NOT NULL,
	Address NVARCHAR(60) NOT NULL,
	City NVARCHAR(50) NOT NULL,
	ContactName NVARCHAR(50),
	ContactTitle NVARCHAR(50),
	Phone NVARCHAR(24),
	HomePage NVARCHAR(MAX),
	WhoChanged NVARCHAR(50) NOT NULL,
	LastModified DATE DEFAULT GETDATE() NOT NULL)

CREATE TABLE SupplierArchive (
	CompanyName NVARCHAR(50) NOT NULL,
	CompanyNIP NVARCHAR(10) NOT NULL,
	Address NVARCHAR(60) NOT NULL,
	City NVARCHAR(50) NOT NULL,
	ContactName NVARCHAR(50),
	ContactTitle NVARCHAR(50),
	Phone NVARCHAR(24),
	HomePage NVARCHAR(MAX),
	WhoChanged NVARCHAR(50) NOT NULL,
	LastModified DATE NOT NULL,
	SupplierID INT NOT NULL,
	ArchiveID INT IDENTITY(1,1) PRIMARY KEY)

CREATE TRIGGER RemoveSupplierTrigger ON Supplier
AFTER DELETE AS
	INSERT INTO SupplierArchive (CompanyName, CompanyNIP, Address, City, ContactName, ContactTitle, Phone, HomePage, WhoChanged, LastModified, SupplierID)
		SELECT CompanyName, CompanyNIP, Address, City, ContactName, ContactTitle, Phone, HomePage, WhoChanged, GETDATE(), SupplierID FROM deleted
*/

/* Product Aspect

CREATE TABLE MeasureUnit (
	UnitID INT IDENTITY(1,1) PRIMARY KEY,
	Code NVARCHAR(5) NOT NULL )

CREATE TABLE ProductCategory (
	CategoryID INT IDENTITY(1,1) PRIMARY KEY,
	CategoryName NVARCHAR(50) NOT NULL,
	Description NVARCHAR(MAX) )

CREATE TABLE Product (
	ProductID INT IDENTITY(1,1) PRIMARY KEY,
	ProductName NVARCHAR(50) NOT NULL,
	InStock DECIMAL(9, 4) DEFAULT 0.0 NOT NULL,
	UnitID INT NOT NULL,
	CategoryID INT,
	SupplierID INT,
	WhoChanged NVARCHAR(50) NOT NULL,
	LastModified DATE DEFAULT GETDATE() NOT NULL,
	FOREIGN KEY(UnitID) REFERENCES MeasureUnit(UnitID),
	FOREIGN KEY(CategoryID) REFERENCES ProductCategory(CategoryID) ON DELETE SET NULL,
	FOREIGN KEY(SupplierID) REFERENCES Supplier(SupplierID) ON DELETE SET NULL )

CREATE TABLE ProductArchive (
	ProductName NVARCHAR(50) NOT NULL,
	UnitCode NVARCHAR(5) NOT NULL,
	CategoryName NVARCHAR(50),
	SupplierName NVARCHAR(50),
	WhoChanged NVARCHAR(50) NOT NULL,
	LastModified DATE NOT NULL,
	ProductID INT NOT NULL,
	ArchiveID INT IDENTITY(1,1) PRIMARY KEY)

CREATE TRIGGER RemoveProductTrigger ON Product
AFTER DELETE AS
	INSERT INTO ProductArchive (ProductName, UnitCode, CategoryName, SupplierName, WhoChanged, LastModified, ProductID)
		SELECT d.ProductName, mu.Code, pc.CategoryName, s.CompanyName, d.WhoChanged, GETDATE(), d.ProductID
			FROM deleted as d
			LEFT JOIN MeasureUnit AS mu ON d.UnitID = mu.UnitID
			LEFT JOIN ProductCategory AS pc ON d.CategoryID = pc.CategoryID
			LEFT JOIN Supplier AS s ON d.SupplierID = s.SupplierID
*/