CREATE TABLE TimecardFile (
    EmployeeName VARCHAR(50),     -- Employee's full name
    EmployeeNumber INT,           -- Unique identifier for the employee
    DateWorked DATE,              -- Date when the work was performed
    EarningsCode VARCHAR(20),     -- Type of earnings (e.g., Regular, Overtime, etc.)
    JobWorked VARCHAR(50),        -- Job title of the employee during the work period
    DeptWorked INT,               -- Department ID where the work was carried out
    Hours DECIMAL(5, 2),          -- Number of hours worked
    Rate DECIMAL(10, 2),          -- Hourly rate of the employee
    Bonus DECIMAL(10, 2)          -- Any additional bonus earned
);

INSERT INTO TimecardFile (EmployeeName, EmployeeNumber, DateWorked, EarningsCode, JobWorked, DeptWorked, Hours, Rate, Bonus) VALUES
('Kyle James', 110011, '1/1/2023', 'Regular', 'Laborer', 001, 8, 15.5, 0),
('Kyle James', 110011, '1/2/2023', 'Regular', 'Laborer', 001, 8, 15.5, 0),
('Kyle James', 110011, '1/3/2023', 'Regular', 'Laborer', 001, 8, 15.5, 0),
('Kyle James', 110011, '1/4/2023', 'Regular', 'Laborer', 001, 8, 15.5, 0),
('Kyle James', 110011, '1/5/2023', 'Regular', 'Laborer', 001, 8, 15.5, 0),
('Kyle James', 110011, '1/6/2023', 'Overtime', 'Laborer', 001, 8, 15.5, 0),
('Jane Smith', 120987, '1/1/2023', 'Regular', 'Scrapper', 002, 10, 17, 0),
('Jane Smith', 120987, '1/2/2023', 'Regular', 'Scrapper', 002, 10, 17, 0),
('Jane Smith', 120987, '1/3/2023', 'Regular', 'Scrapper', 002, 10, 17, 0),
('Jane Smith', 120987, '1/4/2023', 'Regular', 'Scrapper', 002, 10, 17, 0),
('Jane Smith', 120987, '1/5/2023', 'Overtime', 'Scrapper', 002, 6, 17, 0),
('Jane Smith', 120987, '1/6/2023', 'Overtime', 'Scrapper', 002, 6, 17, 0),
('Jane Smith', 120987, '1/7/2023', 'Double time', 'Scrapper', 002, 5, 17, 0),
('Amy Penn', 100002, '1/1/2023', 'Regular', 'Foreman', 003, 8, 17.75, 0),
('Amy Penn', 100002, '1/2/2023', 'Regular', 'Foreman', 003, 10, 17.75, 0),
('Amy Penn', 100002, '1/3/2023', 'Regular', 'Foreman', 003, 10, 17.75, 0),
('Amy Penn', 100002, '1/4/2023', 'Regular', 'Foreman', 003, 10, 17.75, 0),
('Amy Penn', 100002, '1/5/2023', 'Overtime', 'Foreman', 003, 5, 17.75, 125),
('Amy Penn', 100002, '1/6/2023', 'Overtime', 'Foreman', 003, 5, 17.75, 175);

CREATE TABLE rate_table (
    Job VARCHAR(50),                -- Job title, e.g., Laborer, Scrapper, Foreman
    Dept VARCHAR(10),               -- Department ID as a string
    Effective_Start DATE,           -- Start date of the job's effective period
    Effective_End DATE,             -- End date of the job's effective period
    Hourly_Rate DECIMAL(10, 2)      -- Hourly rate for the job
);

INSERT INTO rate_table (Job, Dept, Effective_Start, Effective_End, Hourly_Rate) VALUES
('Laborer', '001', '2023-01-01', '2024-01-01', 18.75),
('Laborer', '002', '2023-01-01', '2024-01-01', 17.85),
('Scrapper', '001', '2022-03-01', '2023-03-01', 19.45),
('Scrapper', '001', '2023-04-01', '2024-01-01', 20.45),
('Scrapper', '002', '2022-03-01', '2023-03-01', 20.55),
('Scrapper', '002', '2023-04-01', '2024-01-01', 21.60),
('Scrapper', '003', '2022-03-01', '2023-03-01', 22.15),
('Scrapper', '003', '2023-04-01', '2024-01-01', 24.15),
('Foreman', '001', '2023-01-01', '2024-01-01', 13.55),
('Foreman', '002', '2023-01-01', '2024-01-01', 14.50),
('Foreman', '003', '2023-01-01', '2024-01-01', 15.60);

