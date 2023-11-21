using MongoGogo.Connection;
using TicketingSystemModel.Ticketing;
using TicketManager.Services.Event.GenerateRandom;

namespace TicketManager.Services.Event
{
    public class EventService
    {
        private readonly IGoCollection<EventEntity> _eventCollection;
        private readonly IGoCollection<TicketEntity> _ticketCollection;

        public EventService(IGoCollection<EventEntity> eventCollection,
                            IGoCollection<TicketEntity> ticketCollection)
        {
            this._eventCollection = eventCollection;
            this._ticketCollection = ticketCollection;
        }

        internal async Task<GenerateRandomResponse> GenerateRandom(GenerateRandomRequest request)
        {
            GenerateRandomModel generateRandomModel = DistributeEvents(request.GenerateEventCount);

            await GenerateConcert(generateRandomModel.ConcertCount, request.GenerateTicketCountPerEvent);
            await GenerateExhibition(generateRandomModel.ExhibitionCount, request.GenerateTicketCountPerEvent);

            return new GenerateRandomResponse
            {
                ConcertCount = generateRandomModel.ConcertCount,
                ExhibitionCount = generateRandomModel.ExhibitionCount
            };
        }

        private async Task GenerateConcert(int concertCount, int generateTicketCountPerEvent)
        {
            if (concertCount == 0) return;

            for (int loopIndex = 0; loopIndex < concertCount; loopIndex++)
            {
                string title = await GetUniqueConcertTitle(ConcertInfoGenerator.GetNames());
                var startAt = RandomDateGenerator.GenerateRandomDateTime();

                //產event
                EventEntity eventEntity = new EventEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Detail = new EventDetail
                    {
                        Title = title,
                        Description = GenerateDescription(ConcertInfoGenerator.GetDescriptions())
                    },
                    Schedule = new EventSchedule
                    {
                        StartAt = startAt,
                        EndAt = startAt.AddHours(new Random().Next(1, 4))
                    },
                    Metadata = new EventMetadata
                    {
                        CreatedAt = DateTime.UtcNow,
                        Type = EventType.Concert
                    }
                };

                //產ticket
                List<TicketEntity> ticketEntities = GenerateTickets(generateTicketCountPerEvent, eventEntity.Id, eventEntity.Schedule.StartAt);

                await _eventCollection.InsertOneAsync(eventEntity);
                await _ticketCollection.InsertManyAsync(ticketEntities);
            }
        }

        private async Task GenerateExhibition(int exhibitionCount, int generateTicketCountPerEvent)
        {
            if (exhibitionCount == 0) return;

            for (int loopIndex = 0; loopIndex < exhibitionCount; loopIndex++)
            {
                string title = await GetUniqueConcertTitle(ExhibitionGenerator.GetNames());
                var startAt = RandomDateGenerator.GenerateRandomDateTime();

                //產event
                EventEntity eventEntity = new EventEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Detail = new EventDetail
                    {
                        Title = title,
                        Description = GenerateDescription(ExhibitionGenerator.GetDescriptions())
                    },
                    Schedule = new EventSchedule
                    {
                        StartAt = startAt,
                        EndAt = startAt.AddHours(new Random().Next(1, 4))
                    },
                    Metadata = new EventMetadata
                    {
                        CreatedAt = DateTime.UtcNow,
                        Type = EventType.Exhibition
                    }
                };

                //產ticket
                List<TicketEntity> ticketEntities = GenerateTickets(generateTicketCountPerEvent, eventEntity.Id, eventEntity.Schedule.StartAt);

                await _eventCollection.InsertOneAsync(eventEntity);
                await _ticketCollection.InsertManyAsync(ticketEntities);
            }
        }

        /// <summary>
        /// 平均分配需要生產的concert與exhibition數量
        /// </summary>
        /// <param name="totalEventCount"></param>
        /// <returns></returns>
        private static GenerateRandomModel DistributeEvents(int totalEventCount)
        {
            var model = new GenerateRandomModel();
            model.ConcertCount = totalEventCount / 2;
            model.ExhibitionCount = totalEventCount / 2;

            //需求數量是奇數的話，最後一個就隨機配
            if (totalEventCount % 2 != 0)
            {
                if (new Random().Next(2) == 0) model.ConcertCount += 1;
                else model.ExhibitionCount += 1;
            }

            return model;
        }

        /// <summary>
        /// 產資料庫中沒有的title。
        /// 如果樣本集不夠的話還是會重複。
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetUniqueConcertTitle(List<string> names)
        {
            var currentConcertNames = (await _eventCollection.FindAsync(filter: _ => true,
                                                                       projection: projecter => projecter.Include(@event => @event.Detail.Title))).Select(@event => @event.Detail.Title).ToList();

            var availableNames = names.Except(currentConcertNames).ToList();

            if(availableNames.Any())
            {
                return availableNames[new Random().Next(availableNames.Count)];
            }
            else
            {
                return names[new Random().Next(names.Count)];
            }
        }

        private static string GenerateDescription(List<string> list)
        {
            return list[new Random().Next(list.Count)];
        }

        private static List<TicketEntity> GenerateTickets(int generateTicketCountPerEvent, string eventId, DateTime eventStartAt)
        {
            var midnightToday = DateTime.UtcNow.Date;
            
            return Enumerable.Range(0, generateTicketCountPerEvent).Select(_ => new TicketEntity
            {
                TicketId = Guid.NewGuid().ToString(),
                EventId = eventId,
                SaleUserInfo = new TicketSaleUserInfo
                {
                    IsSold = TicketStatus.Available,
                    UserId = null
                },
                PurchaseInfo = new TicketPurchaseInfo
                {
                    //為了測試方便起見，startTime總是過去的時點
                    SaleStartTime = midnightToday.AddDays(-10).AddDays(new Random().Next(1, 5)),
                    //為了方便起見，不考慮分批賣ticket的場景。都假設賣到event的開場為止
                    SaleEndTime = eventStartAt
                }
            }).ToList();
        }
    }
}
