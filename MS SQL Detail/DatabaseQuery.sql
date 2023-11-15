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

INSERT INTO AccountGroup (GroupName, IsAdmin, CanChanged, WhoChanged) VALUES (N'System Admin', 1, 0, N'SysAdmin')
INSERT INTO Account (Username, GroupID, CanChanged, WhoChanged) VALUES (N'SysAdmin', 1, 0, N'SysAdmin')
INSERT INTO Password (AccountID, Value, CanChanged, WhoChanged) VALUES (1, N'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 1, N'SysAdmin')
*/