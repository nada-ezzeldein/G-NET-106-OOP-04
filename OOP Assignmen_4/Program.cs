namespace OOP_Assignmen_4
{
    internal class Program
    {
        #region Interfaces
        public interface ITrackable
        {
            string GetTrackingStatus();
        }

        public interface IInsurable
        {
            decimal CalculateInsurance();
        }
        #endregion
        #region Functions From Last Assignment 
        public struct DeliveryAddress
        {
            public string City;
            public string Street;
            public int BuildingNumber;

            public DeliveryAddress(string city, string street, int buildingNumber)
            {
                City = city;
                Street = street;
                BuildingNumber = buildingNumber;
            }

            public string GetFullAddress()
            {
                return $"{BuildingNumber} {Street}, {City}";
            }
        }

        #region Create an Abstract Shipment class Part2
        public abstract class Shipment
        {
            private string trackingCode;
            private string description;
            private decimal weight;
            private decimal deliveryFee;

            public Shipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
            {

                this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNVALID" : trackingCode;
                this.description = string.IsNullOrWhiteSpace(description) ? "NVALID" : description;
                this.weight = weight > 0 ? weight : 1.0m;
                this.deliveryFee = deliveryFee > 0 ? deliveryFee : 10.0m;
                Destination = destination;
            }

            public Shipment(string trackingCode)
            : this(trackingCode, "Unknown", 1.0m, 50.0m, new DeliveryAddress("Default City", "Default St", 1))
            {
            }

            public string TrackingCode
            {
                get { return trackingCode; }
                private set
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        trackingCode = value;
                    }
                }
            }

            public string Description
            {
                get { return description; }
                set
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        description = value;
                    }
                }
            }

            public decimal Weight
            {
                get { return weight; }
                set
                {
                    if (value > 0)
                    {
                        weight = value;
                    }
                }
            }

            public decimal DeliveryFee
            {
                get { return deliveryFee; }
                private set
                {
                    if (value > 0)
                    {
                        deliveryFee = value;
                    }
                }
            }

            public DeliveryAddress Destination { get; set; }
            public abstract decimal EstimatedCost { get; }
            public abstract void PrintShipment();
            public void UpdateDeliveryFee(decimal newFee)
            {
                if (newFee > 0)
                {
                    deliveryFee = newFee;
                }
            }

            public void UpdateWeight(decimal newWeight)
            {
                if (newWeight > 0)
                {
                    weight = newWeight;
                }
            }

            public void UpdateWeight(decimal baseWeight, decimal extraPackingWeight)
            {
                decimal totalWeight = baseWeight + extraPackingWeight;
                if (totalWeight > 0)
                {
                    weight = totalWeight;
                }
            }
       
        }
        #endregion

        #region DeliveryCenter class (Edited)
        public class DeliveryCenter
        {
            private string centerName;
            private Shipment[] shipments;
            private int count;
            public DeliveryCenter(string centerName = "Main Center")
            {
                this.centerName = centerName;
                shipments = new Shipment[20];
                count = 0;
            }

            public Driver AssignedDriver { get; set; }
            public Shipment this[int index]
            {
                get
                {
                    if (shipments == null || index < 0 || index >= count)
                    {
                        return null;
                    }
                    return shipments[index];
                }
                set
                {
                    if (shipments != null && index >= 0 && index < count)
                    {
                        shipments[index] = value;
                    }
                }
            }


            public Shipment this[string trackingCode]
            {
                get
                {
                    if (shipments == null || string.IsNullOrWhiteSpace(trackingCode))
                    {
                        return null;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        if (shipments[i].TrackingCode != null &&
                            shipments[i].TrackingCode.Equals(trackingCode, StringComparison.OrdinalIgnoreCase))
                        {
                            return shipments[i];
                        }
                    }

                    return null;
                }
            }

            public bool AddShipment(Shipment shipment)
            {
                if (shipments == null)
                {
                    shipments = new Shipment[20];
                }

                if (count >= 20 || shipment == null)
                {
                    return false;
                }

                shipments[count] = shipment;
                count++;
                return true;
            }
            public bool RemoveShipment(string trackingCode)
            {
                if (string.IsNullOrWhiteSpace(trackingCode) || count == 0)
                {
                    return false;
                }

                int indexToRemove = -1;
                for (int i = 0; i < count; i++)
                {
                    if (shipments[i]?.TrackingCode != null &&
                        shipments[i].TrackingCode.Equals(trackingCode, StringComparison.OrdinalIgnoreCase))
                    {
                        indexToRemove = i;
                        break;
                    }
                }
                if (indexToRemove == -1)
                {
                    return false;
                }
                for (int i = indexToRemove; i < count - 1; i++)
                {
                    shipments[i] = shipments[i + 1];
                }
                shipments[count - 1] = null;
                count--;

                return true;
            }

            public void PrintAllShipments()
            {
                Console.WriteLine($"--- Delivery Center: {centerName} ---");
                Console.WriteLine($"Total Shipments: {count} / 20");
                Console.WriteLine(new string('=', 30));

                if (count == 0)
                {
                    Console.WriteLine("No shipments available in this center.");
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (shipments[i] != null)
                        {
                            shipments[i].PrintShipment();
                        }
                    }
                }
            }
        }
        #endregion
       
        #region Updated StandardShipment Class
        public class StandardShipment : Shipment, ITrackable, IInsurable
        {
            public StandardShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
                : base(trackingCode, description, weight, deliveryFee, destination)
            {
            }

            public StandardShipment(string trackingCode)
                : base(trackingCode)
            {
            }


            public override decimal EstimatedCost
            {
                get
                {
                    return DeliveryFee + (Weight * 5m);
                }
            }
            public override void PrintShipment()
            {
                Console.WriteLine($"Tracking Code: {TrackingCode}");
                Console.WriteLine($"Description: {Description}");
                Console.WriteLine($"Weight: {Weight}");
                Console.WriteLine($"Delivery Fee: {DeliveryFee}");
                Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
                Console.WriteLine($"Estimated Cost: {EstimatedCost}");
                Console.WriteLine(new string('-', 30));
            }
            public string GetTrackingStatus()
            {
                return $"Standard Shipment [{TrackingCode}] is currently In Transit.";
            }

            public decimal CalculateInsurance()
            {
                return EstimatedCost * 0.05m;
            }
        }
        #endregion

        #region ExpressShipment Class
        public class ExpressShipment : Shipment, ITrackable, IInsurable
        {
            private decimal extraFee;

            public decimal ExtraFee
            {
                get { return extraFee; }
                set
                {
                    if (value >= 0)
                    {
                        extraFee = value;
                    }
                }
            }

            public ExpressShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, decimal extraFee)
                : base(trackingCode, description, weight, deliveryFee, destination)
            {
                ExtraFee = extraFee >= 0 ? extraFee : 0m;
            }
            public override decimal EstimatedCost
            {
                get
                {
                    return DeliveryFee + (Weight * 5m) + ExtraFee;
                }
            }

            public override void PrintShipment()
            {
                Console.WriteLine($"Tracking Code: {TrackingCode}");
                Console.WriteLine($"Description: {Description}");
                Console.WriteLine($"Weight: {Weight}");
                Console.WriteLine($"Delivery Fee: {DeliveryFee}");
                Console.WriteLine($"Extra Fee: {ExtraFee}");
                Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
                Console.WriteLine($"Estimated Cost: {EstimatedCost}");
                Console.WriteLine(new string('-', 30));
            }
            public string GetTrackingStatus()
            {
                return $"Express Shipment [{TrackingCode}] is Out for Delivery.";
            }

            public decimal CalculateInsurance()
            {
                return EstimatedCost * 0.10m;
            }
        }
        #endregion

        #region InternationalShipment Class
        public class InternationalShipment : Shipment, ITrackable, IInsurable
        {
            private string destinationCountry;
            private decimal customsFee;

            public string DestinationCountry
            {
                get { return destinationCountry; }
                set
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        destinationCountry = value;
                    }
                }
            }

            public decimal CustomsFee
            {
                get { return customsFee; }
                set
                {
                    if (value >= 0)
                    {
                        customsFee = value;
                    }
                }
            }

            public InternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, string destinationCountry, decimal customsFee)
                : base(trackingCode, description, weight, deliveryFee, destination)
            {
                DestinationCountry = string.IsNullOrWhiteSpace(destinationCountry) ? "Unknown" : destinationCountry;
                CustomsFee = customsFee >= 0 ? customsFee : 0m;
            }

            public override decimal EstimatedCost
            {
                get
                {
                    return DeliveryFee + (Weight * 5m) + CustomsFee;
                }
            }

            public virtual void GenerateCustomsReport()
            {
                Console.WriteLine($"Generating standard customs report : {DestinationCountry}");
            }
            public override void PrintShipment()
            {
                Console.WriteLine($"Tracking Code: {TrackingCode}");
                Console.WriteLine($"Description: {Description}");
                Console.WriteLine($"Weight: {Weight}");
                Console.WriteLine($"Delivery Fee: {DeliveryFee}");
                Console.WriteLine($"Destination Country: {DestinationCountry}");
                Console.WriteLine($"Customs Fee: {CustomsFee}");
                Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
                Console.WriteLine($"Estimated Cost: {EstimatedCost}");
                Console.WriteLine(new string('-', 30));
            }
            public string GetTrackingStatus()
            {
                return $"International Shipment [{TrackingCode}] is in Customs Clearance at {DestinationCountry}.";
            }

            public decimal CalculateInsurance()
            {
                return (EstimatedCost + CustomsFee) * 0.15m;
            }
        }
        #region CompletedShipment
        public sealed class CompletedShipment : Shipment
        {
            public CompletedShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
                : base(trackingCode, description, weight, deliveryFee, destination)
            {
            }

            public CompletedShipment(string trackingCode)
                : base(trackingCode)
            {
            }
            public override decimal EstimatedCost
            {
                get
                {
                    return DeliveryFee + (Weight * 5m);
                }
            }
            public override void PrintShipment()
            {
                Console.WriteLine("[Status: Completed Shipment]");
                Console.WriteLine($"Tracking Code: {TrackingCode}");
                Console.WriteLine($"Description: {Description}");
                Console.WriteLine($"Weight: {Weight}");
                Console.WriteLine($"Delivery Fee: {DeliveryFee}");
                Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
                Console.WriteLine($"Estimated Cost: {EstimatedCost}");
                Console.WriteLine(new string('-', 30));
            }
        }
        #endregion
        #endregion

        #region PriorityInternationalShipment
        public class PriorityInternationalShipment : InternationalShipment
        {
            public PriorityInternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, string destinationCountry, decimal customsFee)
                : base(trackingCode, description, weight, deliveryFee, destination, destinationCountry, customsFee)
            {
            }
            public sealed override void GenerateCustomsReport()
            {
                Console.WriteLine($"[PRIORITY EXPEDITED] Generating express customs report for {DestinationCountry}");
            }
            public override void PrintShipment()
            {
                Console.WriteLine("[Priority International Shipment]");
                base.PrintShipment();
            }
        }
        #endregion 

        #region Create DeliveryHelper
        public static class DeliveryHelper
        {
            public static void PrintShipmentDetails(Shipment shipment)
            {
                if (shipment != null)
                {
                    shipment.PrintShipment();
                }
                else
                {
                    Console.WriteLine("The shipment details cannot be displayed because the shipment is null.");
                }
            }
        }
        #endregion

        #region Driver
        public class Driver
        {
            public string Name { get; set; }

            public Driver(string name)
            {
                Name = name;
            }
        }
        #endregion
        #endregion
        static void Main(string[] args)
        {
            #region Question 1
            // a)  What is Abstraction in Object-Oriented Programming?
            //  simplifying complex systems by modeling classes based on the essential properties and behaviors of objects while hiding unnecessary details.

            // b)  Why is abstraction considered one of the four pillars of OOP ?
            //  because it allows developers to focus on the essential features of an object while ignoring irrelevant details.
            #endregion

            #region Question 2
            //  a)  What is the difference between an Abstract Class and an Interface?
            //  An abstract class can have both abstract and concrete methods, while an interface can only have abstract methods.

            //  b)  When would you choose an Interface instead of an Abstract Class?
            //  When you want to define a contract that multiple classes can implement, regardless of their inheritance hierarchy.

            //  c)  Can a class inherit from multiple abstract classes? Can it implement multiple interfaces?
            //  A class cannot inherit from multiple abstract classes, but it can implement multiple interfaces.
            #endregion
        }
    }
}
