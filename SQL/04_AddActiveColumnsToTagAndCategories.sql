ALTER TABLE Tag 
Add Active Bit;

UPDATE Tag 
SET Active = 1;

ALTER TABLE Category
ADD Active Bit;

UPDATE Category
SET Active = 1;