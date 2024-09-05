-- Create the ChangeLog table
CREATE TABLE IF NOT EXISTS ChangeLog (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Type VARCHAR(50),
  ChangedRow INT,
  ChangedBy VARCHAR(50),
  CreatedDate DATETIME
);

-- Create an index on CreatedDate
CREATE INDEX IF NOT EXISTS IDX_CreatedDate
ON ChangeLog (CreatedDate);