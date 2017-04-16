﻿namespace TrainReservation.Domain.Services
{
    public class TicketOffice
    {
        private readonly IProvideBookingReferences bookingReferenceProvider;
        private readonly IProvideTrainData trainDataProvider;

        public TicketOffice(IProvideBookingReferences bookingReferenceProvider, IProvideTrainData trainDataProvider)
        {
            this.bookingReferenceProvider = bookingReferenceProvider;
            this.trainDataProvider = trainDataProvider;
        }

        public Reservation MakeReservation(ReservationRequest request)
        {
            var train = trainDataProvider.GetTrainSnapshot(request.TrainId);
            var option = train.Reserve(request.SeatCount);

            if (option.IsFullfiled)
            {
                // Call the various services to validate the transaction
                var bookingReference = bookingReferenceProvider.GetBookingReference();

                trainDataProvider.MarkSeatsAsReserved(request.TrainId, bookingReference, option.ReservedSeats);
                return new Reservation(request.TrainId, bookingReference, option.ReservedSeats);
            }
            else
            {
                return new Reservation(request.TrainId);
            }
        }
    }
}