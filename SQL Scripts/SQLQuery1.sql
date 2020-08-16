SELECT f.id first_name_id, f.first_name first_names, l.id last_name_id, l.last_name last_name
FROM first_names f
INNER JOIN last_names l ON f.id = l.id