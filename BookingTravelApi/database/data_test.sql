-- =============================================
-- INSERT TEST DATA FOR BOOKING TRAVEL API
-- =============================================

-- 1. Roles (Phải tạo trước vì Users cần RoleId)
INSERT INTO `Roles` (`Id`, `Title`) VALUES
(1, 'Admin'),
(2, 'Staff'),
(3, 'Customer');

-- 2. Users
INSERT INTO `Users` (`Id`, `RoleId`, `Password`, `Money`, `BankNumber`, `Bank`, `Name`, `Phone`, `AvatarPath`, `BankBranch`) VALUES
(1, 1, '$2a$11$hashed_password_admin', 50000000, '1234567890', 'Vietcombank', 'Nguyễn Văn Admin', '0901234567', '/avatars/admin.jpg', 'Chi nhánh Đồng Nai'),
(2, 2, '$2a$11$hashed_password_staff', 10000000, '0987654321', 'Techcombank', 'Trần Thị Staff', '0912345678', '/avatars/staff.jpg', 'Chi nhánh Biên Hòa'),
(3, 3, '$2a$11$hashed_password_customer', 20000000, '1122334455', 'BIDV', 'Lê Văn Customer', '0923456789', '/avatars/customer.jpg', 'Chi nhánh TP.HCM');

-- 3. Staffs (Liên kết với Users)
INSERT INTO `Staffs` (`UserId`, `Code`, `IsActive`, `CCCD`, `Address`, `DateOfBirth`, `StartWorkingDate`, `CCCDIssueDate`, `CCCD_front_path`, `CCCD_back_path`, `EndWorkingDate`) VALUES
(2, 'STAFF001', 1, '079123456789', '123 Nguyễn Văn Trỗi, Biên Hòa', '1990-05-15 00:00:00', '2020-01-01 00:00:00', '2019-12-01 00:00:00', '/cccd/staff_front.jpg', '/cccd/staff_back.jpg', '2030-12-31 00:00:00');

-- 4. Tours
INSERT INTO `Tours` (`Id`, `TotalRevenue`, `PercentDeposit`, `Day`, `Title`, `Price`, `Description`) VALUES
(1, 0, 30, 3, 'Tour Đà Lạt 3 ngày 2 đêm', 3500000, 'Khám phá thành phố ngàn hoa với nhiều điểm tham quan hấp dẫn');

-- 5. TourImages
INSERT INTO `TourImages` (`Id`, `TourId`, `Path`) VALUES
(1, 1, '/images/tours/dalat_01.jpg'),
(2, 1, '/images/tours/dalat_02.jpg');

-- 6. Locations
INSERT INTO `Locations` (`Id`, `Name`) VALUES
(1, 'Đà Lạt'),
(2, 'Hồ Chí Minh');

-- 7. Places
INSERT INTO `Places` (`Id`, `LocationId`, `Name`) VALUES
(1, 1, 'Hồ Xuân Hương'),
(2, 1, 'Thác Datanla');

-- 8. LocationActivities
INSERT INTO `LocationActivities` (`Id`, `PlaceId`, `Name`) VALUES
(1, 1, 'Đi thuyền kayak'),
(2, 2, 'Chơi trò cảm giác mạnh');

-- 9. TourLocations (Many-to-Many: Tour và Location)
INSERT INTO `TourLocations` (`TourId`, `LocationId`) VALUES
(1, 1);

-- 10. Activities
INSERT INTO `Activities` (`Id`, `Action`) VALUES
(1, 'Tham quan'),
(2, 'Chụp ảnh'),
(3, 'Ăn uống');

-- 11. ActivityAndLocations (Many-to-Many: Activity và LocationActivity)
INSERT INTO `ActivityAndLocations` (`ActivityId`, `LocationActivityId`) VALUES
(1, 1),
(2, 1),
(3, 2);

-- 12. DayOfTours
INSERT INTO `DayOfTours` (`Id`, `TourId`, `Day`, `Title`, `Description`) VALUES
(1, 1, 1, 'Ngày 1: TP.HCM - Đà Lạt', 'Khởi hành từ TP.HCM, đến Đà Lạt vào buổi chiều'),
(2, 1, 2, 'Ngày 2: Tham quan Đà Lạt', 'Tham quan các điểm nổi tiếng tại Đà Lạt'),
(3, 1, 3, 'Ngày 3: Đà Lạt - TP.HCM', 'Tham quan thêm và trở về TP.HCM');

-- 13. DayActivities (Composite key: DayOfTourId, ActivityId, LocationActivityId)
INSERT INTO `DayActivities` (`DayOfTourId`, `ActivityId`, `LocationActivityId`, `time`) VALUES
(2, 1, 1, '2025-11-15 08:00:00'),
(2, 2, 1, '2025-11-15 10:00:00'),
(2, 3, 2, '2025-11-15 14:00:00');

-- 14. Schedules
INSERT INTO `Schedules` (`Id`, `TourId`, `StartDate`, `EndDate`, `OpenDate`, `MaxSlot`, `FinalPrice`, `GatheringTime`, `Code`, `Desposit`) VALUES
(1, 1, '2025-11-15 06:00:00', '2025-11-17 18:00:00', '2025-10-20 00:00:00', 20, 3500000, '2025-11-15 05:30:00', 'SCH-DL-001', 1050000);

-- 15. Guides (Junction table: Staff và Schedule)
INSERT INTO `Guides` (`StaffId`, `ScheduleId`) VALUES
(2, 1);

-- 16. Status
INSERT INTO `Status` (`Id`, `Name`) VALUES
(1, 'Pending'),
(2, 'Confirmed'),
(3, 'Cancelled'),
(4, 'Completed');

-- 17. Bookings
INSERT INTO `Bookings` (`Id`, `ScheduleId`, `UserId`, `StatusId`, `NumPeople`, `Code`, `Email`, `Phone`, `TotalPrice`, `CountChangeLeft`, `CreatedAt`) VALUES
(1, 1, 3, 2, 2, 'BK-20251016-001', 'customer@example.com', '0923456789', 7000000, 3, '2025-10-16 10:30:00');

-- 18. Reviews
INSERT INTO `Reviews` (`Id`, `UserId`, `Rating`, `Content`, `ScheduleId`, `CreatedAt`) VALUES
(1, 3, 5, 'Tour rất tuyệt vời! Hướng dẫn viên nhiệt tình, lịch trình hợp lý. Sẽ quay lại lần sau.', 1, '2025-11-18 20:00:00');

-- 19. Helpfuls (Users đánh dấu review hữu ích)
INSERT INTO `Helpfuls` (`UserId`, `ReviewId`) VALUES
(1, 1);

-- 20. Favorites (Users yêu thích Tours)
INSERT INTO `Favorites` (`UserId`, `TourId`) VALUES
(3, 1);

-- 21. UserCompletedSchedules (Users đã hoàn thành Schedule)
INSERT INTO `UserCompletedSchedules` (`UserId`, `ScheduleId`) VALUES
(3, 1);

-- 22. Notifications
INSERT INTO `Notifications` (`Id`, `UserId`, `Content`, `IsRead`, `CreatedAt`) VALUES
(1, 3, 'Booking của bạn đã được xác nhận! Mã booking: BK-20251016-001', 1, '2025-10-16 11:00:00'),
(2, 3, 'Tour Đà Lạt sẽ khởi hành vào ngày 15/11/2025. Vui lòng có mặt đúng giờ!', 0, '2025-11-10 09:00:00');

-- =============================================
-- VERIFY DATA
-- =============================================

-- Check counts
SELECT 'Roles' as TableName, COUNT(*) as Count FROM Roles
UNION ALL
SELECT 'Users', COUNT(*) FROM Users
UNION ALL
SELECT 'Staffs', COUNT(*) FROM Staffs
UNION ALL
SELECT 'Tours', COUNT(*) FROM Tours
UNION ALL
SELECT 'TourImages', COUNT(*) FROM TourImages
UNION ALL
SELECT 'Locations', COUNT(*) FROM Locations
UNION ALL
SELECT 'Places', COUNT(*) FROM Places
UNION ALL
SELECT 'LocationActivities', COUNT(*) FROM LocationActivities
UNION ALL
SELECT 'TourLocations', COUNT(*) FROM TourLocations
UNION ALL
SELECT 'Activities', COUNT(*) FROM Activities
UNION ALL
SELECT 'ActivityAndLocations', COUNT(*) FROM ActivityAndLocations
UNION ALL
SELECT 'DayOfTours', COUNT(*) FROM DayOfTours
UNION ALL
SELECT 'DayActivities', COUNT(*) FROM DayActivities
UNION ALL
SELECT 'Schedules', COUNT(*) FROM Schedules
UNION ALL
SELECT 'Guides', COUNT(*) FROM Guides
UNION ALL
SELECT 'Status', COUNT(*) FROM Status
UNION ALL
SELECT 'Bookings', COUNT(*) FROM Bookings
UNION ALL
SELECT 'Reviews', COUNT(*) FROM Reviews
UNION ALL
SELECT 'Helpfuls', COUNT(*) FROM Helpfuls
UNION ALL
SELECT 'Favorites', COUNT(*) FROM Favorites
UNION ALL
SELECT 'UserCompletedSchedules', COUNT(*) FROM UserCompletedSchedules
UNION ALL
SELECT 'Notifications', COUNT(*) FROM Notifications;