CREATE TABLE Venues (
    VenueID INT PRIMARY KEY IDENTITY,
    VenueName NVARCHAR(100),
    Location NVARCHAR(200),
    Capacity INT,
    Description NVARCHAR(500),
    ImageURL NVARCHAR(500)  
);

ALTER TABLE Venues ADD IsAvailble BIT DEFAULT 1;

CREATE TABLE Events (
    EventID INT PRIMARY KEY IDENTITY,
    EventName NVARCHAR(100),
    EventDate DATETIME,
    VenueID INT,
    EventType NVARCHAR(50),
    Description NVARCHAR(500),
    TicketPrice DECIMAL(10, 2),
    FOREIGN KEY (VenueID) REFERENCES Venues(VenueID),
	FOREIGN KEY (EventTypeID) REFERENCES EventType(EventTypeID) --Step 3B
);

--Q3 Step1: EventType Table
CREATE TABLE EventType(
	EventTypeID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL
);

-- Recreate the Bookings table with VenueID
CREATE TABLE Bookings (
    BookingID INT PRIMARY KEY IDENTITY,
    CustomerID INT,
    EventID INT NOT NULL,
    VenueID INT NOT NULL,
    BookingDate DATETIME,
    SeatsBooked INT,
    BookingStatus NVARCHAR(50),

    FOREIGN KEY (EventID) REFERENCES Events(EventID),
    FOREIGN KEY (VenueID) REFERENCES Venues(VenueID)
);

--Q3 Step2: Insert into
INSERT INTO EventsType (Name)
VALUES ('Conference'), ('Wedding'), ('Naming'), ('Birthday'), ('Concert');


INSERT INTO Venues (VenueName, Location, Capacity, Description, ImageURL)
VALUES ('Grand Hall', '45 Event Ave, Cape Town', 300, 'Spacious venue for weddings and concerts.', 'https://example.com/images/grandhall.jpg');


INSERT INTO Events (EventName, EventDate, VenueID, EventType, Description, TicketPrice)
VALUES ('Jazz Night', '2025-04-10 19:00:00', 1, 'Music', 'An evening of live jazz music.', 150.00);


-- Insert Sample Data
INSERT INTO Bookings (CustomerID, EventID, VenueID, BookingDate, SeatsBooked, BookingStatus)
VALUES 
    (1, 1, 1, GETDATE(), 2, 'Confirmed');




SELECT*
	FROM Venues

	SELECT*
	FROM Events

	SELECT*
	FROM Bookings

	SELECT*
	FROM EventsType