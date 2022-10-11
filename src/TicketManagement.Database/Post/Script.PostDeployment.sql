--- Venue
insert into dbo.Venue
values ('First venue', 'First venue address', '123 45 678 90 12', 'Name first venue')

--- Layout
insert into dbo.Layout
values (1, 'First layout', 'Name first layout'),
(1, 'Second layout', 'Name second layout')

--- Area
insert into dbo.Area
values (1, 'First area of first layout', 1, 1),
(1, 'Second area of first layout', 1, 1)

--- Seat
insert into dbo.Seat
values (1, 1, 1),
(1, 1, 2),
(1, 1, 3),
(1, 2, 2),
(2, 1, 1),
(1, 2, 1)

--- Event
insert into dbo.Event
values ('First event','Event', 1, '2030-01-01','2033-01-01', 'https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900', '11:30:00'),
('Second event','Event1', 1, '2034-01-01','2034-06-06', 'https://static.kinoafisha.info/k/movie_posters/canvas/800x1200/upload/movie_posters/3/4/9/8362943/1f273c077a116fc68dd219e88ca47079.jpg', '19:30:00'),
('Third event','Event3', 1, '2035-02-02','2035-06-06', 'https://plaqat.ru/images/47675.jpg', '19:30:00')

--- EventArea
insert into dbo.EventArea
values (1, 'First event area', 1, 1, 1),
(2, 'Second event area', 2, 2, 2),
(3, 'Third event area', 3, 3, 3)

--- EventSeat
insert into dbo.EventSeat
values (1,1,1,0),
(2,2,2,0),
(3,3,3,0)
