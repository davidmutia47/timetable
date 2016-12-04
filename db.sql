use timetable2;

-- drop table if exists `unitsDoneByCourse`;
-- drop table if exists `students`;
-- drop table if exists `courses`;

create table if not exists `courses`(
`id` int not null auto_increment,
`initials` varchar(20) not null unique,
`faculty` varchar(20) not null,
primary key(id)
);

-- drop table if exists `students`;
create table if not exists `students`(
`regno` varchar(20) not null,
`name` varchar(50) not null,
`course` int not null,
`email` varchar(30) not null,
`phone` varchar(20) not null,
primary key(regno),
foreign key(course) references courses(id) on delete cascade 
);

-- drop table if exists `units`;
create table if not exists `units`(
`code` varchar(10) not null,
`name` varchar(30) not null,
primary key(code)
);

-- drop table if exists `lecturers`;
create table if not exists `lecturers`(
`id` int(15) not null,
`name` varchar(30) not null,
`faculty` varchar(20) not null,
primary key(id)
);

-- drop table if exists `rooms`;
create table if not exists `rooms`(
`id` int not null auto_increment,
`number` varchar(12) not null,
`location` varchar(20) not null,
primary key(id)
);

create table if not exists `unitsDoneByCourse`(
`id` int not null auto_increment,
`unit` varchar(10) not null,
`course` int not null,
`lecturer` int(15) not null,
`room` int not null,
`day` int(1) not null,
`timeFrom` time not null,
`timeTo` time not null,
primary key(id),
foreign key(unit) references units(code) on delete cascade ,
foreign key(course) references courses(id) on delete cascade ,
foreign key(lecturer) references lecturers(id) on delete cascade ,
foreign key(room) references rooms(id) on delete cascade 
 
);

CREATE  TABLE `users` (
  `id` INT NOT NULL ,
  `pass` VARCHAR(45) NOT NULL ,
  PRIMARY KEY (`id`) );