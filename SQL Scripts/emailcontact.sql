

SELECT c.first_name AS FirstName,c.middle_name, c.last_name AS LastName
FROM dbo.contacts as c
INNER JOIN dbo.contacts_email_addresses as ce
ON ce.contact_id = c.id
WHERE ce.contact_id = 5;