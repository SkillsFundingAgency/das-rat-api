namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string SubregionName { get; set; }
        public string RegionName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public static implicit operator Region(Entities.Region source)
        {
            if (source == null)
            {
                return null;
            }

            return new Region
            {
                Id = source.Id,
                SubregionName = source.SubregionName,
                RegionName = source.RegionName,
                Latitude = source.Latitude,
                Longitude = source.Longitude
            };
        }
    }
}
