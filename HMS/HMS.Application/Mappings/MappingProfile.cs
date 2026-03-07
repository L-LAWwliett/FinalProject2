using AutoMapper;
using HMS.Domain.Entities;
using HMS.Application.DTOs.Hotel;
using HMS.Application.DTOs.Room;
using HMS.Application.DTOs.Guest;
using HMS.Application.DTOs.Reservation;
using HMS.Application.DTOs.Manager;

namespace HMS.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Hotel Mappings
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<CreateHotelDto, Hotel>();
            CreateMap<UpdateHotelDto, Hotel>();



            // Room Mappings
            CreateMap<Domain.Entities.Room, RoomDto>().ReverseMap();
            CreateMap<CreateRoomDto, Domain.Entities.Room>();
            CreateMap<UpdateRoomDto, Domain.Entities.Room>();

            // Guest Mappings
            CreateMap<Domain.Entities.Guest, GuestDto>().ReverseMap();
            CreateMap<CreateGuestDto, Domain.Entities.Guest>();
            CreateMap<UpdateGuestDto, Domain.Entities.Guest>();

            // Reservation Mappings
            CreateMap<Domain.Entities.Reservation, ReservationDto>().ReverseMap();
            CreateMap<CreateReservationDto, Domain.Entities.Reservation>();
            CreateMap<UpdateReservationDto, Domain.Entities.Reservation>();

            // Manager Mappings
            CreateMap<Domain.Entities.Manager, ManagerDto>().ReverseMap();
            CreateMap<CreateManagerDto, Domain.Entities.Manager>();
            CreateMap<UpdateManagerDto, Domain.Entities.Manager>();


        }

    }
}