using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.Files.Models.Out
{
    public class UserDiskSpace
    {
        private ulong free;
        private ulong used;
        private ulong total;

        public Unit FreeUnit { get; set; }
        public Unit UsedUnit { get; set; }
        public Unit TotalUnit { get; set; }

        public string FreeUnitString { get; set; }
        public string UsedUnitString { get; set; }
        public string TotalUnitString { get; set; }

        public ulong Free
        {
            get => free;
            set
            {
                FreeUnit = CheckUnit(value);
                free = CalculateByUnit(value, FreeUnit);
                FreeUnitString = FreeUnit.ToString();
            }
        }

        public ulong Used
        {
            get => used;
            set
            {
                UsedUnit = CheckUnit(value);
                used = CalculateByUnit(value, UsedUnit);
                UsedUnitString = UsedUnit.ToString();
            }
        }

        public ulong Total
        {
            get => total;
            set
            {
                TotalUnit = CheckUnit(value);
                total = CalculateByUnit(value, TotalUnit);
                TotalUnitString = TotalUnit.ToString();
            }
        }

        private Unit CheckUnit(ulong value)
        {
            if (value >= (ulong)Unit.TB)
            {
                return Unit.TB;
            }
            else if (value >= (ulong)Unit.GB)
            {
                return Unit.GB;
            }
            else if (value >= (ulong)Unit.MB)
            {
                return Unit.MB;
            }
            else if (value >= (ulong)Unit.kB)
            {
                return Unit.kB;
            }
            else
            {
                return Unit.bytes;
            }
        }

        private ulong CalculateByUnit(ulong value, Unit unit)
        {
            switch (unit)
            {
                case Unit.TB:
                    {
                        value /= (ulong)Unit.TB;
                        break;
                    }
                case Unit.GB:
                    {
                        value /= (ulong)Unit.GB;
                        break;
                    }
                case Unit.MB:
                    {
                        value /= (ulong)Unit.MB;
                        break;
                    }
                case Unit.kB:
                    {
                        value /= (ulong)Unit.kB;
                        break;
                    }
                default:
                    {
                        value /= (ulong)Unit.bytes;
                        break;
                    }
            }

            return value;
        }

        public enum Unit : ulong
        {
            bytes = 1,
            kB = 1000,
            MB = 1000000,
            GB = 1000000000,
            TB = 1000000000000
        }
    }
}
