USE [master]
GO
/****** Object:  Database [BookingBadmintonSystem]    Script Date: 6/14/2024 1:47:23 PM ******/
CREATE DATABASE [BookingBadmintonSystem]

USE [BookingBadmintonSystem]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountName] [nvarchar](50) NULL,
	[Password] [nchar](15) NULL,
	[FullName] [nvarchar](50) NULL,
	[Phone] [nchar](12) NULL,
	[Email] [nchar](50) NULL,
	[RoleID] [int] NULL,
	[Status] [bit] NULL,
	[Image] [nvarchar](50) NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Amenities]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Amenities](
	[AmenitiID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Amenity] PRIMARY KEY CLUSTERED 
(
	[AmenitiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [unique_description] UNIQUE NONCLUSTERED 
(
	[Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AmenityCourt]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmenityCourt](
	[AmenityCourtID] [int] IDENTITY(1,1) NOT NULL,
	[AmenityID] [int] NULL,
	[CourtID] [int] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_AmenityCourt] PRIMARY KEY CLUSTERED 
(
	[AmenityCourtID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Area]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[AreaID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [nvarchar](250) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Complexe] PRIMARY KEY CLUSTERED 
(
	[AreaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [unique_location] UNIQUE NONCLUSTERED 
(
	[Location] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[BookingID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[BookingTypeID] [int] NULL,
	[PlayerQuantity] [int] NULL,
	[TotalPrice] [float] NULL,
	[Note] [nvarchar](500) NULL,
	[Status] [bit] NULL,
	[TotalHours] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[MonthsDuration] [int] NULL,
 CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
(
	[BookingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingDetail]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingDetail](
	[BookingDetailID] [int] IDENTITY(1,1) NOT NULL,
	[BookingID] [int] NULL,
	[SlotID] [int] NULL,
	[Date] [datetime] NULL,
	[Status] [bit] NULL,
	[SubCourtID] [int] NULL,
	[CheckInTime] [datetime] NULL,
	[TimeReducedInMinutes] [int] NULL,
 CONSTRAINT [PK_BookingDetail] PRIMARY KEY CLUSTERED 
(
	[BookingDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingType]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingType](
	[BookingTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_BookingType] PRIMARY KEY CLUSTERED 
(
	[BookingTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CheckIns]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CheckIns](
	[CheckInID] [int] IDENTITY(1,1) NOT NULL,
	[BookingDetailID] [int] NULL,
	[CheckInTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Context] [nvarchar](450) NULL,
	[UserId] [int] NULL,
	[Status] [bit] NULL,
	[Image] [nvarchar](50) NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Court]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Court](
	[CourtId] [int] IDENTITY(1,1) NOT NULL,
	[AreaId] [int] NULL,
	[CourtName] [nvarchar](50) NULL,
	[OpenTime] [nchar](20) NULL,
	[CloseTime] [nchar](20) NULL,
	[Rules] [nvarchar](max) NULL,
	[Status] [bit] NULL,
	[Image] [nvarchar](50) NULL,
	[ManagerId] [int] NULL,
	[Title] [nvarchar](100) NULL,
	[Address] [nvarchar](max) NULL,
	[TotalRate] [float] NULL,
	[Rate] [float] NULL,
 CONSTRAINT [PK_Court] PRIMARY KEY CLUSTERED 
(
	[CourtId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[BookingID] [int] NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentAmount] [float] NULL,
	[TotalAmount] [float] NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[PostID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL,
	[Context] [nvarchar](max) NULL,
	[TotalRate] [float] NULL,
	[Status] [bit] NULL,
	[Image] [nchar](50) NULL,
	[Title] [nchar](500) NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RatingCourt]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RatingCourt](
	[RatingCourtId] [int] IDENTITY(1,1) NOT NULL,
	[CourtId] [int] NULL,
	[UserId] [int] NULL,
	[RatingValue] [float] NULL,
 CONSTRAINT [PK_RatingCourt] PRIMARY KEY CLUSTERED 
(
	[RatingCourtId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RatingPost]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RatingPost](
	[RatingID] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NULL,
	[UserId] [int] NULL,
	[RatingValue] [float] NULL,
 CONSTRAINT [PK_RatingPost] PRIMARY KEY CLUSTERED 
(
	[RatingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nchar](7) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SlotTime]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SlotTime](
	[SlotId] [int] IDENTITY(1,1) NOT NULL,
	[StartTime] [nchar](20) NULL,
	[EndTime] [nchar](20) NULL,
	[Price] [float] NULL,
	[Status] [bit] NULL,
	[ManagerId] [int] NULL,
	[SubCourtId] [int] NULL,
	[CourtId] [int] NULL,
 CONSTRAINT [PK_SlotTime] PRIMARY KEY CLUSTERED 
(
	[SlotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubCourt]    Script Date: 6/14/2024 1:47:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCourt](
	[SubCourtId] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NULL,
	[CourtId] [int] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_CourtNumber] PRIMARY KEY CLUSTERED 
(
	[SubCourtId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Role]
GO
ALTER TABLE [dbo].[AmenityCourt]  WITH CHECK ADD  CONSTRAINT [FK_AmenityCourt_Amenities] FOREIGN KEY([AmenityID])
REFERENCES [dbo].[Amenities] ([AmenitiID])
GO
ALTER TABLE [dbo].[AmenityCourt] CHECK CONSTRAINT [FK_AmenityCourt_Amenities]
GO
ALTER TABLE [dbo].[AmenityCourt]  WITH CHECK ADD  CONSTRAINT [FK_AmenityCourt_Court1] FOREIGN KEY([CourtID])
REFERENCES [dbo].[Court] ([CourtId])
GO
ALTER TABLE [dbo].[AmenityCourt] CHECK CONSTRAINT [FK_AmenityCourt_Court1]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Account] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Account]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_BookingType1] FOREIGN KEY([BookingTypeID])
REFERENCES [dbo].[BookingType] ([BookingTypeID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_BookingType1]
GO
ALTER TABLE [dbo].[BookingDetail]  WITH CHECK ADD  CONSTRAINT [FK_BookingDetail_Booking] FOREIGN KEY([BookingID])
REFERENCES [dbo].[Booking] ([BookingID])
GO
ALTER TABLE [dbo].[BookingDetail] CHECK CONSTRAINT [FK_BookingDetail_Booking]
GO
ALTER TABLE [dbo].[BookingDetail]  WITH CHECK ADD  CONSTRAINT [FK_BookingDetail_SlotTime] FOREIGN KEY([SlotID])
REFERENCES [dbo].[SlotTime] ([SlotId])
GO
ALTER TABLE [dbo].[BookingDetail] CHECK CONSTRAINT [FK_BookingDetail_SlotTime]
GO
ALTER TABLE [dbo].[BookingDetail]  WITH CHECK ADD  CONSTRAINT [FK_BookingDetail_SubCourt1] FOREIGN KEY([SubCourtID])
REFERENCES [dbo].[SubCourt] ([SubCourtId])
GO
ALTER TABLE [dbo].[BookingDetail] CHECK CONSTRAINT [FK_BookingDetail_SubCourt1]
GO
ALTER TABLE [dbo].[CheckIns]  WITH CHECK ADD  CONSTRAINT [FK_CheckIns_BookingDetail] FOREIGN KEY([BookingDetailID])
REFERENCES [dbo].[BookingDetail] ([BookingDetailID])
GO
ALTER TABLE [dbo].[CheckIns] CHECK CONSTRAINT [FK_CheckIns_BookingDetail]
GO
ALTER TABLE [dbo].[Court]  WITH CHECK ADD  CONSTRAINT [FK_Court_Account] FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Court] CHECK CONSTRAINT [FK_Court_Account]
GO
ALTER TABLE [dbo].[Court]  WITH CHECK ADD  CONSTRAINT [FK_Court_Area1] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([AreaID])
GO
ALTER TABLE [dbo].[Court] CHECK CONSTRAINT [FK_Court_Area1]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Booking] FOREIGN KEY([BookingID])
REFERENCES [dbo].[Booking] ([BookingID])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Booking]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_Post_Account]
GO
ALTER TABLE [dbo].[RatingCourt]  WITH CHECK ADD  CONSTRAINT [FK_RatingCourt_Account] FOREIGN KEY([UserId])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[RatingCourt] CHECK CONSTRAINT [FK_RatingCourt_Account]
GO
ALTER TABLE [dbo].[RatingCourt]  WITH CHECK ADD  CONSTRAINT [FK_RatingCourt_Court] FOREIGN KEY([CourtId])
REFERENCES [dbo].[Court] ([CourtId])
GO
ALTER TABLE [dbo].[RatingCourt] CHECK CONSTRAINT [FK_RatingCourt_Court]
GO
ALTER TABLE [dbo].[RatingPost]  WITH CHECK ADD  CONSTRAINT [FK_RatingPost_Account] FOREIGN KEY([UserId])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[RatingPost] CHECK CONSTRAINT [FK_RatingPost_Account]
GO
ALTER TABLE [dbo].[RatingPost]  WITH CHECK ADD  CONSTRAINT [FK_RatingPost_Post] FOREIGN KEY([PostId])
REFERENCES [dbo].[Post] ([PostID])
GO
ALTER TABLE [dbo].[RatingPost] CHECK CONSTRAINT [FK_RatingPost_Post]
GO
ALTER TABLE [dbo].[SlotTime]  WITH CHECK ADD  CONSTRAINT [FK_SlotTime_Account] FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[SlotTime] CHECK CONSTRAINT [FK_SlotTime_Account]
GO
ALTER TABLE [dbo].[SlotTime]  WITH CHECK ADD  CONSTRAINT [FK_SlotTime_Court] FOREIGN KEY([CourtId])
REFERENCES [dbo].[Court] ([CourtId])
GO
ALTER TABLE [dbo].[SlotTime] CHECK CONSTRAINT [FK_SlotTime_Court]
GO
ALTER TABLE [dbo].[SlotTime]  WITH CHECK ADD  CONSTRAINT [FK_SlotTime_SubCourt] FOREIGN KEY([SubCourtId])
REFERENCES [dbo].[SubCourt] ([SubCourtId])
GO
ALTER TABLE [dbo].[SlotTime] CHECK CONSTRAINT [FK_SlotTime_SubCourt]
GO
ALTER TABLE [dbo].[SubCourt]  WITH CHECK ADD  CONSTRAINT [FK_SubCourt_Court] FOREIGN KEY([CourtId])
REFERENCES [dbo].[Court] ([CourtId])
GO
ALTER TABLE [dbo].[SubCourt] CHECK CONSTRAINT [FK_SubCourt_Court]
GO
USE [master]
GO
ALTER DATABASE [BookingBadmintonSystem] SET  READ_WRITE 
GO
