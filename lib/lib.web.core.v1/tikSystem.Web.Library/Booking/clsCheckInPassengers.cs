using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Data;
using System.IO;

namespace tikSystem.Web.Library
{
    [Serializable()]
    public class CheckInPassengers : CollectionBase
    {
        [XmlIgnore]
        public agentservice.TikAeroXMLwebservice objService = null;
        public CheckinPassenger this[int index]
        {
            get { return (CheckinPassenger)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Add(CheckinPassenger value)
        {
            return this.List.Add(value);
        }
        public string GetPassengerDetails(string strLanCode)
        {
            ServiceClient obj = new ServiceClient();
            Library objLi = new Library();

            obj.objService = objService;

            return obj.GetPassengerDetails(string.Empty,
                                           string.Empty,
                                           string.Empty,
                                           XmlHelper.Serialize(this, false),
                                           true,
                                           true,
                                           true,
                                           true, // baggage
                                           true,
                                           true,
                                           false,
                                           strLanCode,
                                           string.Empty);
        }
        public bool CheckInSave(string strMappingXml, string strBaggageXml, string strSeatAssignmentXml, string strPassengerXml, string strServiceXml, string strRemarkXml, string strBookingSegmentXml, string strFeeXml)
        {
            ServiceClient obj = new ServiceClient();
            Library objLi = new Library();

            obj.objService = objService;

            return obj.CheckInSave(strMappingXml,
                                   strBaggageXml,
                                   strSeatAssignmentXml,
                                   strPassengerXml,
                                   strServiceXml,
                                   strRemarkXml,
                                   strBookingSegmentXml,
                                   strFeeXml);
        }

        public Services GetSpecialService(string language_rcd)
        {
            ServiceClient obj = new ServiceClient();
            Library objLi = new Library();

            obj.objService = objService;

            return obj.GetSpecialServices(language_rcd);
        }

        #region Seat Assignment Function

        public bool AssignSeatPercentage(DataTable Seat, ref Mappings mappings, ref string message, bool bWindowSeat, bool bAisleSeat)
        {
            DataView objDV = null;

            int i = 0;
            int NumberOfBay = 0;
            int BayNumber = 0;
            int SeatCount = 0;
            int iPaxNumber = 0;
            int TotalSeat = 0;
            double SeatPercentage = 0;
            double CurrentPercentage = 0;

            string strResult = string.Empty;

            bool bAssign = false;
            bool bPercentageCount = false;

            objDV = null;

            if (Seat.Rows.Count > 0)
            {
                iPaxNumber = 0;
                foreach (Mapping mp in mappings)
                {
                    //Count Number of passenger
                    if ((mp.free_seating_flag == 1))
                    {
                    }
                    else
                    {
                        if (mp.e_ticket_flag == 1 & mp.ticket_number.Length > 0)
                        {
                            if (mp.passenger_check_in_status_rcd != "OFFLOADED")
                            {
                                if (mp.seat_number.Length == 0)
                                {
                                    iPaxNumber = iPaxNumber + 1;
                                }
                            }
                        }
                    }
                }

                objDV = Seat.DefaultView;
                {
                    if (objDV.Count > 0)
                    {
                        NumberOfBay = Convert.ToInt16(objDV[0]["number_of_bays"]);
                        BayNumber = 0;

                        objDV.RowFilter = "location_type_rcd = 'ST' and " + "seat_number is null";
                        SeatCount = objDV.Count;

                        objDV.RowFilter = "location_type_rcd = 'ST'";
                        TotalSeat = objDV.Count;

                        if (SeatCount == TotalSeat)
                        {
                            bPercentageCount = false;
                        }
                        else
                        {
                            bPercentageCount = true;
                        }

                        //No seat is assign yet
                        TotalSeat = 0;
                        SeatCount = 0;

                        for (i = 1; i <= NumberOfBay + 1; i++)
                        {
                            if (BayNumber == 0)
                            {
                                BayNumber = i;
                            }

                            if (i <= NumberOfBay)
                            {
                                if (bPercentageCount == false)
                                {
                                    objDV.RowFilter = "location_type_rcd = 'ST' AND " + "bay_number = " + i;
                                    SeatCount = objDV.Count;
                                    if (TotalSeat < SeatCount)
                                    {
                                        TotalSeat = SeatCount;
                                        BayNumber = i;
                                    }
                                }
                                else
                                {
                                    objDV.RowFilter = "location_type_rcd = 'ST' and " + "seat_number is null and " + "bay_number = " + i;
                                    SeatCount = objDV.Count;

                                    objDV.RowFilter = "location_type_rcd = 'ST' and " + "bay_number = " + i;
                                    TotalSeat = objDV.Count;

                                    SeatPercentage = (SeatCount / TotalSeat) * 100;
                                    if (CurrentPercentage < SeatPercentage)
                                    {
                                        CurrentPercentage = SeatPercentage;
                                        BayNumber = i;
                                    }
                                }

                            }
                            else
                            {
                                objDV.RowFilter = "location_type_rcd = 'ST' and " + "seat_number is null and " + "handicapped_flag = 0 and " + "emergency_exit_flag = 0 and " + "blocked_flag = 0 and " + "block_b2c_flag = 0 and " + "stretcher_flag = 0 and " + "bay_number = " + BayNumber;

                                if (iPaxNumber > objDV.Count)
                                {
                                    //Seat not enough take total seat instead of each bay
                                    objDV.RowFilter = "location_type_rcd = 'ST' and " + "seat_number is null and " + "handicapped_flag = 0 and " + "emergency_exit_flag = 0 and " + "blocked_flag = 0 and " + "block_b2c_flag = 0 and " + "stretcher_flag = 0";
                                }

                            }
                        }
                        strResult = DynamicAssign(objDV, iPaxNumber, false, ref mappings, bWindowSeat, bAisleSeat);
                    }
                }
            }

            bAssign = SaveSeat(strResult, ref mappings);

            if ((objDV == null) == false)
            {
                objDV.Dispose();
            }

            return bAssign;
        }
        public bool AssignSeatByBay(DataTable Seat, ref Mappings mappings, ref string message, bool bWindowSeat, bool bAisleSeat)
        {
            DataView objDV = null;

            int i = 0;
            int NumberOfBay = 0;
            int BayNumber = 0;
            int SeatCount = 0;
            int iPaxNumber = 0;

            string strResult = string.Empty;
            string strRowCondition = string.Empty;
            bool bAssign = false;

            objDV = null;

            if (Seat.Rows.Count > 0)
            {
                iPaxNumber = 0;
                foreach (Mapping mp in mappings)
                {
                    //Count Number of passenger
                    if ((mp.free_seating_flag == 1))
                    {
                    }
                    else
                    {
                        if (mp.e_ticket_flag == 1 & mp.ticket_number.Length > 0)
                        {
                            if (mp.passenger_check_in_status_rcd != "OFFLOADED")
                            {
                                if (mp.seat_number.Length == 0)
                                {
                                    iPaxNumber = iPaxNumber + 1;
                                }
                            }
                        }
                    }
                }

                objDV = Seat.DefaultView;
                {
                    if (objDV.Count > 0)
                    {
                        NumberOfBay = Convert.ToInt16(objDV[0]["number_of_bays"]) + 1;
                        BayNumber = 0;
                        for (i = 1; i <= NumberOfBay + 1; i++)
                        {
                            if (i == NumberOfBay)
                            {
                                if (BayNumber == 0)
                                {
                                    strRowCondition = string.Empty;
                                }
                                else if (iPaxNumber > SeatCount)
                                {
                                    strRowCondition = string.Empty;
                                }
                                else
                                {
                                    strRowCondition = "and bay_number = " + BayNumber;
                                }
                            }
                            else if (i == NumberOfBay + 1)
                            {
                                //Check If selected bay have enough seat to checkin
                                if (iPaxNumber > objDV.Count)
                                {
                                    //Seat not enough take total seat instead of each bay
                                    strRowCondition = string.Empty;
                                }
                            }
                            else
                            {
                                strRowCondition = "and bay_number = " + i;
                            }

                            objDV.RowFilter = "location_type_rcd = 'ST' and " + "seat_number is null and " + "handicapped_flag = 0 and " + "emergency_exit_flag = 0 and " + "blocked_flag = 0 and " + "block_b2c_flag = 0 and " + "stretcher_flag = 0 " + strRowCondition;

                            if (SeatCount < objDV.Count)
                            {
                                SeatCount = objDV.Count;
                                BayNumber = i;
                            }
                        }

                        strResult = DynamicAssign(objDV, iPaxNumber, false, ref mappings, bWindowSeat, bAisleSeat);
                    }
                }
            }

            bAssign = SaveSeat(strResult, ref mappings);

            if ((objDV == null) == false)
            {
                objDV.Dispose();
            }

            return bAssign;
        }
        public bool AssignSeatFromBack(DataTable Seat, ref Mappings mappings, ref string message, bool bWindowSeat, bool bAisleSeat)
        {
            DataView objDV = null;

            int iPaxNumber = 0;

            string strResult = string.Empty;
            bool bAssign = false;

            objDV = null;

            if (Seat.Rows.Count > 0)
            {
                iPaxNumber = 0;
                foreach (Mapping mp in mappings)
                {
                    //Count Number of passenger
                    if ((mp.free_seating_flag == 1))
                    {
                    }
                    else
                    {
                        if (mp.e_ticket_flag == 1 & mp.ticket_number.Length > 0)
                        {
                            if (mp.passenger_check_in_status_rcd != "OFFLOADED")
                            {
                                if (mp.seat_number.Length == 0)
                                {
                                    iPaxNumber = iPaxNumber + 1;
                                }
                            }
                        }
                    }
                }

                objDV = Seat.DefaultView;
                {
                    objDV.RowFilter = "location_type_rcd = 'ST' and " + "seat_number is null and " + "handicapped_flag = 0 and " + "emergency_exit_flag = 0 and " + "blocked_flag = 0 and " + "block_b2c_flag = 0 and " + "stretcher_flag = 0";
                    if (objDV.Count > 0)
                    {
                        strResult = DynamicAssign(objDV, iPaxNumber, false, ref mappings, bWindowSeat, bAisleSeat);
                        bAssign = SaveSeat(strResult, ref mappings);
                    }
                }
            }

            if ((objDV == null) == false)
            {
                objDV.Dispose();
            }

            return bAssign;
        }
        public bool AssignSeatFromMiddle(DataTable Seat, ref Mappings mappings, string message, bool bWindowSeat, bool bAisleSeat)
        {
            DataView objDV = null;

            int i = 0;
            int MaxRow = 0;
            int DivRow = 0;
            int SeatCount = 0;
            int CountUpper = 0;
            int CountLower = 0;
            int iPaxNumber = 0;

            string strResult = string.Empty;
            string strRowCondition = string.Empty;
            bool bAssign = false;

            objDV = null;

            //check passenger type & OFFLOADED for seat assign from web config
            bool IsSeatOFFLOADEDAllowed = false;
            if (System.Configuration.ConfigurationManager.AppSettings["SeatOFFLOADEDAllowed"] != null)
            {
                IsSeatOFFLOADEDAllowed = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SeatOFFLOADEDAllowed"]);
            }

            if (Seat.Rows.Count > 0)
            {
                iPaxNumber = 0;
                foreach (Mapping mp in mappings)
                {
                    //Count Number of passenger
                    if ((mp.free_seating_flag == 1))
                    {
                    }
                    else
                    {
                       // if (mp.e_ticket_flag == 1 & mp.ticket_number.Length > 0)
                        if (true)
                        {
                            if (mp.passenger_check_in_status_rcd != "OFFLOADED")
                            {
                                if (mp.seat_number.Length == 0)
                                {
                                    iPaxNumber = iPaxNumber + 1;
                                }
                            }
                            else
                            {
                                //yo 31-01
                                if (IsSeatOFFLOADEDAllowed)
                                {
                                    if (mp.seat_number.Length == 0)
                                    {
                                        iPaxNumber = iPaxNumber + 1;
                                    }
                                }
                            }
                        }
                    }
                }

                objDV = Seat.DefaultView;
                {
                    objDV.RowFilter = "location_type_rcd = 'ST'";
                    SeatCount = objDV.Count;
                    MaxRow = Convert.ToInt16(objDV[objDV.Count - 1]["layout_row"]);
                    if (MaxRow > 0)
                    {
                        DivRow = MaxRow / 2;
                    }

                    for (i = 0; i <= 3; i++)
                    {
                        if (i == 2)
                        {
                            //2 Get selected Part
                            if (CountUpper > CountLower)
                            {
                                if (iPaxNumber > CountUpper)
                                {
                                    strRowCondition = string.Empty;
                                }
                                else
                                {
                                    strRowCondition = "and layout_row <= " + DivRow;
                                }
                            }
                            else
                            {
                                if (iPaxNumber > CountLower)
                                {
                                    strRowCondition = string.Empty;
                                }
                                else
                                {
                                    strRowCondition = "and layout_row > " + DivRow;
                                }
                            }
                        }
                        else if (i == 3)
                        {
                            //Check If selected bay have enough seat to checkin
                            if (iPaxNumber > objDV.Count)
                            {
                                //Seat not enough take total seat instead of each bay
                                strRowCondition = string.Empty;
                            }
                        }
                        else
                        {
                            //0 to 1 Count number of upper and lower part
                            if (i == 0)
                            {
                                strRowCondition = "and layout_row <= " + DivRow;
                            }
                            else
                            {
                                strRowCondition = "and layout_row > " + DivRow;
                            }
                        }

                        objDV.RowFilter = "location_type_rcd = 'ST' and " + "passenger_count = 0 and " + "handicapped_flag = 0 and " + "emergency_exit_flag = 0 and " + "blocked_flag = 0 and " + "block_b2c_flag = 0 and " + "stretcher_flag = 0 " + strRowCondition;
                        //yo
                        //objDV.RowFilter = "location_type_rcd = 'ST' and " + "seat_number is null and " + "emergency_exit_flag = 0 and " + "blocked_flag = 0 and " + "block_b2c_flag = 0 and " + "stretcher_flag = 0 " + strRowCondition;

                        if (i < 2)
                        {
                            if (i == 0)
                            {
                                CountUpper = objDV.Count;
                            }
                            else
                            {
                                CountLower = objDV.Count;
                            }
                        }
                    }

                    if (iPaxNumber > CountUpper & iPaxNumber > CountLower)
                    {
                        strResult = DynamicAssign(objDV, iPaxNumber, false, ref mappings, bWindowSeat, bAisleSeat);
                    }
                    else if (CountUpper > CountLower)
                    {
                        strResult = DynamicAssign(objDV, iPaxNumber, false, ref mappings, bWindowSeat, bAisleSeat);
                    }
                    else
                    {
                        strResult = DynamicAssign(objDV, iPaxNumber, true, ref mappings, bWindowSeat, bAisleSeat);
                    }
                }
            }

            //bAssign = SaveSeat(strResult, ref Mappings);
            //yo 
            if (strResult.Length > 3)
            {
                bAssign = SaveSeat(strResult, ref mappings);
                message = bAssign.ToString();
            }
            else
            {
                message = strResult;//306 (Not enough seat for assigning)
            }

            if ((objDV == null) == false)
            {
                objDV.Dispose();
            }

            return bAssign;
        }
        public bool AllPaxHaveSeat(Mappings Mappings, Guid bookingSegmentId, string strPassengerId)
        {
            //int iPaxNumber = 0;
            //int iPaxWithSeat = 0;
            int iNumberOfSeatSelected = 0;

            foreach (Mapping mp in Mappings)
            {
                if (mp.booking_segment_id.Equals(bookingSegmentId))
                {
                    if (mp.passenger_check_in_status_rcd != "OFFLOADED")
                    {
                        if (strPassengerId != "") //check-in alone
                        {
                            if (mp.passenger_id == new Guid(strPassengerId))
                            {
                                if (mp.seat_number.Length == 0)
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }

                        if (mp.seat_number.Length == 0)  //Check-in All
                        {
                            iNumberOfSeatSelected = ++iNumberOfSeatSelected;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (iNumberOfSeatSelected > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private string DynamicAssign(DataView objDV, int PaxNumber, bool bFromUpper, ref Mappings mappings, bool bWindowSeat, bool bAisleSeat)
        {
            StringBuilder stbSeatAssign = new StringBuilder();
            StringBuilder stbActualSeatAssign = new StringBuilder();

            string strSeatNumber = string.Empty;
            string strNotInfant = string.Empty;
            string strInfant = string.Empty;
            string strSeatLocationCondition = string.Empty;
            string[] strNotInfants = null;
            string[] strInfants = null;

            int i = 0;
            int iRow = 0;
            int iCol = 0;
            int iPaxAssign = 0;
            int iPaxToAssign = 0;
            int iRemaindingPax;
            int iPaxNotInfant = 0;
            int iPaxInfant = 0;
            int iPaxAll = 0;
            int iLastLayoutRow = 0;
            int iLastLayoutColumn = 0;
            int iTempRow = 0;
            int iTempCol = 0;
            int iStartIndex = 0;
            int iEndIndex = 0;
            int iColumnCount = 0;
            int iNumberOfSeatCanSelect = 0;
            int iTotalAdult = 0;

            bool bToLeft = false;
            //bool bEnoughSeat = true;
            bool bFindStartIndex = false;
            bool bSecondRound = false;

            //check passenger type : infants or not infants
            if (mappings != null)
            {
                foreach (Mapping mp in mappings)
                {
                    //Check seat_nubmer already have
                    if (mp.seat_number.Length == 0)
                    {
                        switch (mp.passenger_type_rcd.ToUpper())
                        {
                            case "ADULT":
                                strNotInfant += mp.passenger_id + "|";
                                break;
                            case "YOUTH":
                                strNotInfant += mp.passenger_id + "|";
                                break;
                            case "CHD":
                                strNotInfant += mp.passenger_id + "|";
                                break;
                            case "INF":
                                strInfant += mp.passenger_id + "|";
                                break;
                            case "STAFF":
                                strNotInfant += mp.passenger_id + "|";
                                break;
                            default:
                                break;
                        }
                    }
                }

                strNotInfants = strNotInfant.Split('|');
                strInfants = strInfant.Split('|');
                iPaxAll = (strNotInfants.Length - 1) + (strInfants.Length - 1);
                iTotalAdult = strNotInfants.Length - 1;
            }

            if (objDV.Count > 0 && objDV.Count >= iPaxAll)
            {
                if (objDV.Count >= PaxNumber)
                {
                    if (objDV.Count > 0)
                    {
                        iPaxAssign = 0;

                        iRemaindingPax = PaxNumber;
                        iPaxToAssign = PaxNumber;

                        if (bFromUpper == true)
                        {
                            //Start the loop from the first index.
                            i = 0;

                            iLastLayoutRow = Convert.ToInt32(objDV[objDV.Count - 1]["layout_row"]); //last available row
                            iLastLayoutColumn = Convert.ToInt32(objDV[objDV.Count - 1]["layout_column"]); //last available column
                        }
                        else
                        {
                            //start the loop from the last index.
                            i = objDV.Count - 1;

                            iLastLayoutRow = Convert.ToInt32(objDV[0]["layout_row"]); //last available row
                            iLastLayoutColumn = Convert.ToInt32(objDV[0]["layout_column"]); //last available column
                        }

                        DataTable utable = objDV.ToTable(true, new string[] { "flight_id", "layout_row", "layout_column", "seat_row", "seat_column", "location_type_rcd", "passenger_count", "handicapped_flag", "emergency_exit_flag", "stretcher_flag", "blocked_flag", "block_b2c_flag", "window_flag", "aisle_flag" });

                        using (DataView dvClone = new DataView(utable))
                        {
                            do
                            {
                                bFindStartIndex = false;
                                if (iRemaindingPax != 0)
                                {
                                    iRow = Convert.ToInt32(objDV[i]["layout_row"]);
                                    iCol = Convert.ToInt32(objDV[i]["layout_column"]);

                                    if (iTempRow != iRow)
                                    {
                                        iTempRow = iRow;
                                        //bEnoughSeat = true;

                                        //dvClone.RowFilter = "location_type_rcd = 'ST' and seat_number is null and handicapped_flag = 0 and emergency_exit_flag = 0 and stretcher_flag = 0 and blocked_flag = 0 and block_b2c_flag = 0 and layout_row = " + iRow;
                                        dvClone.RowFilter = "location_type_rcd = 'ST' and passenger_count = 0 and handicapped_flag = 0 and emergency_exit_flag = 0 and stretcher_flag = 0 and blocked_flag = 0 and block_b2c_flag = 0 and layout_row = " + iRow;

                                        //Find seat per column.
                                        //  Check if all passenger can be assign to one row.
                                        iColumnCount = dvClone.Count;
                                        //Find first seat
                                        if (bSecondRound == false)
                                        {
                                            if (iColumnCount >= iTotalAdult)
                                            {
                                                //Check position which index to start and which index should end.
                                                iStartIndex = 0;
                                                iEndIndex = 0;
                                                bToLeft = false;
                                                for (int k = 0; k < iColumnCount; k++)
                                                {
                                                    //Find go left or right
                                                    iNumberOfSeatCanSelect = 0;
                                                    if (bAisleSeat == false && bWindowSeat == false)
                                                    {
                                                        iStartIndex = k;
                                                        bToLeft = false;
                                                        iEndIndex = iColumnCount;
                                                        bFindStartIndex = true;
                                                    }
                                                    else
                                                    {
                                                        //Find Start index.
                                                        if (Convert.ToBoolean(dvClone[k]["window_flag"]) == bWindowSeat & bWindowSeat == true)
                                                        {
                                                            iStartIndex = k;
                                                            bFindStartIndex = true;
                                                        }
                                                        else if (Convert.ToBoolean(dvClone[k]["aisle_flag"]) == bAisleSeat & bAisleSeat == true)
                                                        {
                                                            iStartIndex = k;
                                                            bFindStartIndex = true;
                                                        }

                                                        //Find end index.
                                                        if (bFindStartIndex == true)
                                                        {
                                                            if (iStartIndex == 0)
                                                            {
                                                                //Go to right if it is the first index.
                                                                bToLeft = false;
                                                                iEndIndex = iColumnCount;
                                                            }
                                                            else if (((iStartIndex + 1) == iColumnCount) ||
                                                                    (Convert.ToInt32(dvClone[iStartIndex]["layout_column"]) + 1) != Convert.ToInt32(dvClone[iStartIndex + 1]["layout_column"]))
                                                            {
                                                                //Go to left
                                                                bToLeft = true;
                                                                iEndIndex = 0;
                                                            }
                                                            else
                                                            {
                                                                //Go to right
                                                                bToLeft = false;
                                                                iEndIndex = iColumnCount;
                                                            }
                                                        }
                                                    }

                                                    if (bFindStartIndex == true)
                                                    {
                                                        //check available seat.
                                                        if (bToLeft == true)
                                                        {
                                                            //Find number of continus seat to the left that can be select.
                                                            iTempCol = 0;
                                                            for (int l = iStartIndex; l >= iEndIndex; l--)
                                                            {
                                                                if (l == iStartIndex)
                                                                {
                                                                    iTempCol = Convert.ToInt32(dvClone[iStartIndex]["layout_column"]);
                                                                    iNumberOfSeatCanSelect++;
                                                                }
                                                                else if (Convert.ToInt32(dvClone[l]["layout_column"]) == (iTempCol - 1))
                                                                {
                                                                    iTempCol = Convert.ToInt32(dvClone[l]["layout_column"]);
                                                                    iNumberOfSeatCanSelect++;
                                                                }

                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Find number of continus seat to the right that can be select.
                                                            iTempCol = 0;
                                                            for (int l = iStartIndex; l < iEndIndex; l++)
                                                            {
                                                                if (l == iStartIndex)
                                                                {
                                                                    iTempCol = Convert.ToInt32(dvClone[iStartIndex]["layout_column"]);
                                                                    iNumberOfSeatCanSelect++;
                                                                }
                                                                else if (Convert.ToInt32(dvClone[l]["layout_column"]) >= (iTempCol + 1))
                                                                {
                                                                    iTempCol = Convert.ToInt32(dvClone[l]["layout_column"]);
                                                                    iNumberOfSeatCanSelect++;
                                                                }
                                                            }
                                                        }
                                                    }


                                                    if (bFindStartIndex == true && (iNumberOfSeatCanSelect >= iTotalAdult))
                                                    {
                                                        //Start Seat Assignment
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        iStartIndex = 0;
                                                        bFindStartIndex = false;

                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            bFindStartIndex = true;
                                            bToLeft = false;

                                            iStartIndex = 0;
                                            iEndIndex = iColumnCount;
                                            iNumberOfSeatCanSelect = iColumnCount;
                                        }


                                        if ((bFindStartIndex == true && iNumberOfSeatCanSelect >= iTotalAdult) || bSecondRound == true)
                                        {
                                            //Assign passenger to seat.
                                            //  If find both stat and end index them start assign seat.
                                            do
                                            {
                                                strSeatNumber = dvClone[iStartIndex]["seat_row"].ToString() + dvClone[iStartIndex]["seat_column"].ToString();
                                                stbActualSeatAssign.Append("<Seat>");
                                                stbActualSeatAssign.Append("<seat_row>");
                                                stbActualSeatAssign.Append(dvClone[iStartIndex]["seat_row"].ToString());
                                                stbActualSeatAssign.Append("</seat_row>");
                                                stbActualSeatAssign.Append("<seat_column>");
                                                stbActualSeatAssign.Append(dvClone[iStartIndex]["seat_column"].ToString());
                                                stbActualSeatAssign.Append("</seat_column>");
                                                stbActualSeatAssign.Append("<seat_number>");
                                                stbActualSeatAssign.Append(strSeatNumber.Trim());
                                                stbActualSeatAssign.Append("</seat_number>");
                                                stbActualSeatAssign.Append("<passenger_id>");
                                                stbActualSeatAssign.Append(strNotInfants[iPaxNotInfant]);
                                                stbActualSeatAssign.Append("</passenger_id>");
                                                stbActualSeatAssign.Append("</Seat>");

                                                iPaxAssign++;
                                                iPaxNotInfant++;

                                                if (strInfants.Length > iPaxInfant && !string.IsNullOrEmpty(strInfants[iPaxInfant]))
                                                {
                                                    stbActualSeatAssign.Append("<Seat>");
                                                    stbActualSeatAssign.Append("<seat_row>");
                                                    stbActualSeatAssign.Append(dvClone[iStartIndex]["seat_row"].ToString());
                                                    stbActualSeatAssign.Append("</seat_row>");
                                                    stbActualSeatAssign.Append("<seat_column>");
                                                    stbActualSeatAssign.Append(dvClone[iStartIndex]["seat_column"].ToString());
                                                    stbActualSeatAssign.Append("</seat_column>");
                                                    stbActualSeatAssign.Append("<seat_number>");
                                                    stbActualSeatAssign.Append(strSeatNumber.Trim());
                                                    stbActualSeatAssign.Append("</seat_number>");
                                                    stbActualSeatAssign.Append("<passenger_id>");
                                                    stbActualSeatAssign.Append(strInfants[iPaxInfant]);
                                                    stbActualSeatAssign.Append("</passenger_id>");
                                                    stbActualSeatAssign.Append("</Seat>");

                                                    iPaxAssign++;
                                                    iPaxInfant++;
                                                }

                                                if (bToLeft == true)
                                                {
                                                    iStartIndex--;
                                                }
                                                else
                                                {
                                                    iStartIndex++;
                                                }

                                                //Exit loop when index end.
                                                if (iStartIndex == iColumnCount)
                                                {
                                                    break;
                                                }
                                            }
                                            while (iPaxAssign != iPaxToAssign);
                                        }
                                    }

                                    if (iRow == iLastLayoutRow && iCol == iLastLayoutColumn)
                                    {
                                        i = bFromUpper ? -1 : objDV.Count;//i = -1;//iTempStartRow;
                                        bSecondRound = true;
                                    }
                                }

                                if (bFromUpper == true)
                                {
                                    i = i + 1;
                                }
                                else
                                {
                                    i = i - 1;
                                }

                            } while (!(iPaxAssign == iPaxToAssign));
                        }
                    }
                }
            }
            else
            {
                stbActualSeatAssign.Append("306"); //Not enough seat for assigning
            }

            return stbActualSeatAssign.ToString();
        }

        private bool IsAssign(DataView objDv, int iRow, int iCol)
        {
            string TempFilter = objDv.RowFilter;
            {
                objDv.RowFilter = "layout_row = " + iRow + " and " + "layout_column = " + iCol;
                if (objDv.Count > 0)
                {
                    for (int i = 0; i <= objDv.Count - 1; i++)
                    {
                        if (objDv[i]["passenger_id"].ToString().Length > 0)
                        {
                            objDv.RowFilter = TempFilter;
                            return true;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

            objDv.RowFilter = TempFilter;
            return false;
        }

        private bool SaveSeat(string Seat, ref Mappings Mappings)
        {
            int i = 0;
            bool bAssign = false;

            if (Seat.Length > 0)
            {
                XPathDocument xmlDoc = new XPathDocument(new StringReader("<Seats>" + Seat + "</Seats>"));
                XPathNavigator nv = xmlDoc.CreateNavigator();

                Library objLi = new Library();
                i = 1;

                foreach (Mapping mp in Mappings)
                {
                   // if (mp.e_ticket_flag == 1 & mp.ticket_number.Length > 0)
                    if (true)
                    {
                        if (mp.seat_number.Length == 0)
                        {
                            foreach (XPathNavigator n in nv.Select("Seats/Seat[passenger_id='" + mp.passenger_id + "']"))
                            {
                                mp.seat_row = Convert.ToInt32(objLi.getXPathNodevalue(n, "seat_row", Library.xmlReturnType.value));
                                mp.seat_column = objLi.getXPathNodevalue(n, "seat_column", Library.xmlReturnType.value);
                                mp.seat_number = objLi.getXPathNodevalue(n, "seat_number", Library.xmlReturnType.value);
                            }
                            i++;
                        }
                    }
                }
                bAssign = true;
            }
            else
            {
                bAssign = false;
            }
            return bAssign;
        }
        #endregion

        #region Board Passenger Function
        public string BoardPassenger(string strFlightID, string strOrigin, string strBoard, string strUserId, bool bBoard)
        {
            ServiceClient obj = new ServiceClient();

            obj.objService = objService;
            return obj.BoardPassenger(strFlightID, strOrigin, strBoard, strUserId, bBoard);
        }

        #endregion
    }
}
