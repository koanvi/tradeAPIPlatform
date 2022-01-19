--=============================
-- SQLite
SELECT

  id,
  'date',
  sourse,
  'open',
  'close',
  high,
  low,
  volume

FROM candels
WHERE 'close'>0
LIMIT 10;

--=============================

--=============================
drop TABLE candels

--=============================
