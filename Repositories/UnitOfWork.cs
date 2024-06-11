using Repositories.DTO;
using Repositories.Entities;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{

    public class UnitOfWork
    {
        private BookingBadmintonSystemContext _context;
        private AccountRepo _account;
        private RoleRepo _role;
        private AmenityRepo _amenity;
        private BookingRepo _booking;
        private SlotTimeRepo _slotTime;
        private AmenityCourtRepo _amenityCourt;
        private CommentRepo _comment;
        private AreaRepo _areaRepo;
        private CourtRepo _court;
        private BookingDetailsRepo _bookingDetails;
        private BookingTypeRepo _bookingType;
        private PaymentRepo _paymentRepo;
        private PostRepo _post;
        private SubCourtRepo _subCourt;
        private CheckInRepo _checkIn;

        public UnitOfWork() => _context = new BookingBadmintonSystemContext();
        public  CheckInRepo CheckInRepo
        {
            get
            {
                return _checkIn ??= new CheckInRepo();
            }
        }

        public AccountRepo AccountRepo
        {
            get
            {
                return _account ??= new Repositories.AccountRepo();
            }
        }

        public RoleRepo RoleRepo
        {
            get
            {
                return _role ??= new Repositories.RoleRepo();
            }
        }

        public AmenityRepo AmenityRepo
        {
            get
            {
                return _amenity ??= new Repositories.AmenityRepo();
            }
        }

        public BookingRepo BookingRepo
        {
            get
            {
                return _booking ??= new Repositories.BookingRepo();
            }
        }
        public SlotTimeRepo SlotTimeRepo
        {
            get
            {
                return _slotTime ??= new Repositories.SlotTimeRepo();
            }
        }

        public PostRepo PostRepo
        {
            get
            {
                return _post ??= new Repositories.PostRepo();
            }
        }
        public PaymentRepo PaymentRepo
        {
            get
            {
                return _paymentRepo ??= new Repositories.PaymentRepo();
            }
        }

        public BookingTypeRepo BookingTypeRepo
        {
            get
            {
                return _bookingType ??= new Repositories.BookingTypeRepo();
            }
        }

        public BookingDetailsRepo BookingDetailRepo
        {
            get
            {
                return _bookingDetails ??= new Repositories.BookingDetailsRepo();
            }
        }
        public AmenityCourtRepo AmenityCourtRepo
        {
            get
            {
                return _amenityCourt ??= new Repositories.AmenityCourtRepo();
            }
        }

        public CommentRepo CommentRepo
        {
            get
            {
                 return _comment ??= new Repositories.CommentRepo();
            }
        }

        public AreaRepo AreaRepo
        {
            get
            {
                return _areaRepo ??= new Repositories.AreaRepo();
            }
        }

        public CourtRepo CourtRepo
        {
            get
            {
                return _court ??= new Repositories.CourtRepo();
            }
        }
        public SubCourtRepo SubCourtRepo
        {
            get
            {
                return _subCourt ??= new Repositories.SubCourtRepo();
            }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
