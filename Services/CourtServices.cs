using Microsoft.AspNetCore.Http;
using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICourtServices
    {
        public PagedResult<CourtDTOs> GetCourts( int pageNumber, int pageSize);
        CourtGET GetCourtById(int id);
        void UpdateCourt(int courtId, CourtDTOs courtDTO);
        Task<Court> CreateCourtAsync(CourtDTO courtDTO);
        public PagedResult<CourtDTOs> SearchCourts(string searchTerm, int pageNumber, int pageSize);
        bool DeleteCourt(int id);
        Task UploadCourtImageAsync(int courtId, byte[] imageBytes);
        void RateCourt(int courtId, int userId, double rating);
        public string GetCourtImagePath(int courtId);
    }
    public class CourtServices : ICourtServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ImageServices _imageService;
        public CourtServices()
        {
            _unitOfWork ??= new UnitOfWork();
            _imageService = new ImageServices();
        }
        public PagedResult<CourtDTOs> GetCourts(int pageNumber, int pageSize)
        {
            var courts = _unitOfWork.CourtRepo.GetAll();
            var totalItemCount = courts.Count;
            var pagedCourts = courts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var courtDTOs = pagedCourts.Select(c => new CourtDTOs
            {
                CourtId = c.CourtId,
                AreaId = c.AreaId,
                CourtName = c.CourtName,
                OpenTime = c.OpenTime,
                CloseTime = c.CloseTime,
                ManagerId = c.ManagerId,
                Image = c.Image,
                Rules = c.Rules,
                Status = c.Status,
                TotalRate = c.TotalRate,
                Address = c.Address,
                Title = c.Title,
                PriceAvr = c.PricePerHour,
            }).ToList();

            return new PagedResult<CourtDTOs>
            {
                Items = courtDTOs,
                TotalItem = totalItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public CourtGET GetCourtById(int id)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            if (court == null)
            {
                return null;
            }

            var subCourts = _unitOfWork.SubCourtRepo.GetSubCourtByCourtId(court.CourtId) ?? new List<SubCourt>();
            var amenityCourts = _unitOfWork.AmenityCourtRepo.GetAmenityByCourtId(court.CourtId) ?? new List<AmenityCourt>();
            var slotTimes = _unitOfWork.SlotTimeRepo.GetSlotTimeByCourtId(court.CourtId) ?? new List<SlotTime>();

            var subCourtDTOs = subCourts.Select(sc => new SubCourtGet
            {
                SubCourtId = sc.SubCourtId,
                Number = sc.Number,
                Status = sc.Status,
                SlotTimes = slotTimes.Where(st => st.SubCourtId == sc.SubCourtId).Select(st => new SlotTimeDTO
                {
                    SlotId = st.SlotId,
                    StartTime = st.StartTime,
                    EndTime = st.EndTime,
                    WeekdayPrice = st.WeekdayPrice,
                    WeekendPrice = st.WeekendPrice,
                    Status = st.Status,
                }).ToList()
            }).ToList();

            return new CourtGET
            {
                CourtId = court.CourtId,
                CourtName = court.CourtName,
                OpenTime = court.OpenTime,
                CloseTime = court.CloseTime,
                ManagerId = court.ManagerId,
                Rules = court.Rules,
                Image = court.Image,
                Status = court.Status,
                Title = court.Title,
                Address = court.Address,
                TotalRate = court.TotalRate,
                AreaId = court.AreaId,
                SubCourts = subCourtDTOs,
                PriceAvr = court.PricePerHour,
                Amenities = amenityCourts.Select(ac => new AmenityCourtDTO
                {
                    AmenityCourtId = ac.AmenityCourtId,
                    AmenityId = ac.AmenityId,
                    Status = ac.Status
                }).ToList(),
                SlotTimes = slotTimes.Where(st => st.SubCourtId == 0).Select(st => new SlotTimeDTO
                {
                    SlotId = st.SlotId,
                    StartTime = st.StartTime,
                    EndTime = st.EndTime,
                    WeekdayPrice = st.WeekdayPrice,
                    WeekendPrice = st.WeekendPrice,
                    Status = st.Status,
                }).ToList()
            };
        }

        public void UpdateCourt(int id, CourtDTOs courtDTO)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            if (court != null)
            {
                court.AreaId = courtDTO.AreaId;
                court.ManagerId = courtDTO.ManagerId;
                court.Address = courtDTO.Address;
                court.CourtName = courtDTO.CourtName;
                court.OpenTime = courtDTO.OpenTime;
                court.CloseTime = courtDTO.CloseTime;
                court.Rules = courtDTO.Rules;
                court.Status = courtDTO.Status;
                court.PricePerHour = courtDTO.PriceAvr;
                _unitOfWork.CourtRepo.Update(court);
                _unitOfWork.SaveChanges();
            }
        }

        public async Task<Court> CreateCourtAsync(CourtDTO courtDTO)
        {
            var court = new Court
            {
                AreaId = courtDTO.AreaId,
                CourtName = courtDTO.CourtName,
                OpenTime = courtDTO.OpenTime,
                CloseTime = courtDTO.CloseTime,
                ManagerId = courtDTO.ManagerId,
                Rules = courtDTO.Rules,
                Status = true,
                Title = courtDTO.Title,
                Address = courtDTO.Address,
                TotalRate = courtDTO.TotalRate,
                PricePerHour = courtDTO.PriceAvr
            };
            _unitOfWork.CourtRepo.Create(court);
            _unitOfWork.SaveChanges();

            var createdSubCourts = new List<SubCourt>();

            if (courtDTO.SubCourts != null && courtDTO.SubCourts.Any())
            {
                foreach (var subCourt in courtDTO.SubCourts)
                {
                    var newSubCourt = new SubCourt
                    {
                        CourtId = court.CourtId,
                        Number = subCourt.Number,
                        Status = true,
                    };
                    _unitOfWork.SubCourtRepo.Create(newSubCourt);
                    _unitOfWork.SaveChanges();
                    createdSubCourts.Add(newSubCourt);
                }
            }

            if (courtDTO.Amenities != null && courtDTO.Amenities.Any())
            {
                foreach (var amenityCourt in courtDTO.Amenities)
                {
                    if (amenityCourt != null)
                    {
                        var newAmenity = new AmenityCourt
                        {
                            CourtId = court.CourtId,
                            AmenityId = amenityCourt.AmenityId,
                            Status = true
                        };
                        _unitOfWork.AmenityCourtRepo.Create(newAmenity);
                        _unitOfWork.SaveChanges();
                    }
                }
            }

            if (courtDTO.SlotTimes != null && courtDTO.SlotTimes.Any())
            {
                foreach (var subCourt in createdSubCourts)
                {
                    foreach (var slot in courtDTO.SlotTimes)
                    {
                        var slotTime = new SlotTime
                        {
                            StartTime = slot.StartTime,
                            EndTime = slot.EndTime,
                            WeekdayPrice = slot.WeekdayPrice,
                            WeekendPrice = slot.WeekendPrice,
                            CourtId = court.CourtId,
                            ManagerId = court.ManagerId,
                            SubCourtId = subCourt.SubCourtId,
                            Status = true
                        };
                        _unitOfWork.SlotTimeRepo.Create(slotTime);
                        _unitOfWork.SaveChanges();
                    }
                }
            }

            if (!string.IsNullOrEmpty(courtDTO.Image) && courtDTO.Image != "string")
            {
                try
                {
                    var base64Parts = courtDTO.Image.Split(',');
                    if (base64Parts.Length == 2)
                    {
                        var base64Data = base64Parts[1];
                        var imageBytes = Convert.FromBase64String(base64Data);
                        var fileName = Guid.NewGuid().ToString() + ".png";
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        var filePath = Path.Combine(uploadPath, fileName);
                        await File.WriteAllBytesAsync(filePath, imageBytes);

                        court.Image = fileName;
                        _unitOfWork.CourtRepo.Update(court);
                    }
                    else
                    {
                        throw new Exception("Invalid Base64 image format.");
                    }
                }
                catch (FormatException)
                {
                    throw new Exception("Invalid Base64 image format.");
                }
            }
            _unitOfWork.SaveChanges();
            return court;
        }

        public bool DeleteCourt(int id)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            if (court == null)
            {
                return false;
            }
            court.Status = false;
            _unitOfWork.CourtRepo.Update(court);
            _unitOfWork.SaveChanges();
            return true;
        }

        public PagedResult<CourtDTOs> SearchCourts(string searchTerm, int pageNumber, int pageSize)
        {
            var courts = _unitOfWork.CourtRepo.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                courts = courts.Where(c =>
                    c.CourtName.ToLower().Contains(lowerSearchTerm) ||
                    (c.Area != null && c.Area.Location.ToLower().Contains(lowerSearchTerm))).ToList();
            }

            var totalItemCount = courts.Count;
            var pagedCourts = courts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var courtDTOs = pagedCourts.Select(c => new CourtDTOs
            {
                CourtId = c.CourtId,
                AreaId = c.AreaId,
                CourtName = c.CourtName,
                OpenTime = c.OpenTime,
                CloseTime = c.CloseTime,
                Rules = c.Rules,
                Image = c.Image,
                Status = c.Status,
                TotalRate = c.TotalRate,
                Address = c.Address,
                Title = c.Title,
                ManagerId = c.ManagerId,
                PriceAvr = c.PricePerHour
            }).ToList();

            return new PagedResult<CourtDTOs>
            {
                Items = courtDTOs,
                TotalItem = totalItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task UploadCourtImageAsync(int courtId, byte[] imageBytes)
        {
            var court = _unitOfWork.CourtRepo.GetById(courtId);
            if (court == null)
            {
                Console.WriteLine("Court not found.");
                return;
            }

            var fileName = Guid.NewGuid().ToString() + ".png";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Console.WriteLine("Creating uploads directory.");
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);

            court.Image = fileName;

            _unitOfWork.CourtRepo.Update(court);
            _unitOfWork.SaveChanges();

            Console.WriteLine("Image saved successfully.");
        }


        public void RateCourt(int courtId, int userId, double rating)
        {
            var court = _unitOfWork.CourtRepo.GetById(courtId);
            if (court == null || court.Status == false) return;

            var existingRating = _unitOfWork.RatingCourtRepo.GetAll()
                .FirstOrDefault(rc => rc.CourtId == courtId && rc.UserId == userId);

            if (existingRating != null)
            {
                existingRating.RatingValue = rating;
                _unitOfWork.RatingCourtRepo.Update(existingRating);
            }
            else
            {
                var ratingcourt = new RatingCourt
                {
                    CourtId = courtId,
                    UserId = userId,
                    RatingValue = rating
                };
                _unitOfWork.RatingCourtRepo.Create(ratingcourt);
            }

            var countRatings = _unitOfWork.RatingCourtRepo.GetAll().Count(rc => rc.CourtId == courtId);
            var totalRatings = _unitOfWork.RatingCourtRepo.GetAll().Where(rc => rc.CourtId == courtId).Sum(rc => rc.RatingValue);

            court.TotalRate = totalRatings > 0 ? (double)totalRatings / countRatings : 0;
            _unitOfWork.CourtRepo.Update(court);
            _unitOfWork.SaveChanges();
        }

        public string GetCourtImagePath(int courtId)
        {
            var court = _unitOfWork.CourtRepo.GetById(courtId);
            if (court == null)
            {
                throw new Exception("Court not found.");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var imagePath = Path.Combine(uploadPath, court.Image);

            if (!System.IO.File.Exists(imagePath))
            {
                throw new Exception("Image file not found.");
            }

            return imagePath;
        }

    }
}

