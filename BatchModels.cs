using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace StreetPerfect.Models
{
	public class caBatchRunRequest
	{
		public BatchConfig Config { get; set; }
		public string InputData { get; set; }

	}

	public class caBatchRunResponse
	{
		public string Status { get; set; }
		public string Msg { get; set; }
	}

	public class caBatchStatusRequest
	{
		public string BatchId { get; set; }
		public string Msg { get; set; }
	}


	// replace with just BatchStatus
	public class caBatchStatusResponse
	{
		public BatchStatus Status { get; set; }
		public string Msg { get; set; }
	}

	public class BatchStatus
	{
		//public enum Status { NotFound, Provisioning, Pending, Running, Finished, Error }
		public string CurrentStatus { get; set; }
		public string Log { get; set; }
		public DateTime StartTimeUtc { get; set; }
		public DateTime StopTimeUtc { get; set; }
		public string Msg { get; set; }
	}


	public class BatchConfig
	{

		public string PreferredUnitDesignatorKeyword { get; set; } = "SUITE";
		public string PreferredUnitDesignatorStyle { get; set; } = "K";
		public string OutputFormatGuide { get; set; } = "N";
		public string ExceptionReportLevel { get; set; } = "D";
		public string PrintMessageNumbers { get; set; } = "N";
		public string PrintInformationMessages { get; set; } = "Y";
		public string PrintChangeMessages { get; set; } = "Y";
		public string PrintErrorMessages { get; set; } = "Y";
		public string PrintTryMessages { get; set; } = "Y";
		public string PrintOptimizeMessages { get; set; } = "Y";

		/// <summary>
		/// 
		/// </summary>
		public string OptimizeAddress { get; set; } = "S";


		/// <summary>
		///  ProcessErrors has been extended to support the following options
		///  
		///  'M'   module (entry point) trace <br/>
		///  'Q'   function trace + sql statement trace <br/>
		///  'S'   sql statement trace <br/>
		///  'F'   function trace <br/>
		///  'T'   transaction trace: module frequency metrics
		///	 * 'T' requires setting DBOM_EnableDebugTransactionRange
		///  * 0 = all transactions | 1 to 4 transaction set
		///  * t&lt;=1024, 1024&gt;t&lt;=4096, 4096&gt;t&lt;=16384, t&gt;16384
		///  
		///  'Z'   transaction trace + sql statement trace
		/// </summary>
		public string ProcessErrors { get; set; } = "N";
		//public string ReportByCompanyID { get; set; } = "SAMP";
		//public string ReportForCompanyID { get; set; } = "SAMP";
		//public string ReportFileID { get; set; } = "001";
		public int? ErrorTolerance { get; set; } = 2;
		public int? MaximumTryMessages { get; set; } = 5;
		public string CorrectLvrAddress { get; set; } = "Q";
		public string CorrectLvrAmbiguity { get; set; } = "Q";
		public string CorrectRuralAddress { get; set; } = "Q";
		public string ReportAllUnidentified { get; set; } = "Y";
		public string ReportOrphanUdkAsExtraInfo { get; set; } = "Y";


		/// <summary>
		///
		///   function to be performed for this run
		///   "C" for correction, "V" for validation, "P" for parse
		///
		/// </summary>
		public string Function { get; set; } = "C";

		/// <summary>
		///
		/// Specifiy whether to prefix output with the StreetPerfect Correct/Validate status flag.
		///
		/// Canadian: 
		/// * 'I' Invalid
		/// * 'N' Not Correctable
		/// * 'V' Valid
		/// * 'C' Corrected
		/// * 'F' Foreign
		///
		/// "Yes" for yes "No" for no
		///
		/// </summary>
		public string OutputStatusFlag { get; set; } = "Yes";

		/// <summary>
		///
		///   File format to be used for this run:
		///   
		///   "F" for fixed length fields <br/>
		///   "C" for comma delimited fields <br/>
		///   "S" for semicolon delimited fields <br/>
		///   "T" for tab delimited fields <br/>
		///   "U:c" where "c" is a user defined field delimiter, can be any printable character not part of the data stream
		///
		/// </summary>
		public string FileFormat { get; set; } = "F";

		/// <summary>
		///
		///   Address format to be used for this run:
		///
		///   "C" for delivery address line "Component" 
		///   
		/// AddressStreetNumber
		/// * AddressStreetNumberOffset
		/// * AddressStreetNumberLength 
		/// 
		/// AddressStreetName
		/// * AddressStreetNameOffset
		/// * AddressStreetNameLength
		/// 
		/// AddressUnitNumber
		/// * AddressUnitNumberOffset
		/// * AddressUnitNumberLength
		///
		/// "L" for delivery address line "Line" 
		/// 
		/// AddressLine
		/// * AddressLineOffset
		/// * AddressLineLength
		///
		/// </summary>
		public string AddressFormat { get; set; } = "L";

		/// <summary>
		///
		///   First record in the file to start processing
		///   
		///   Note: This is strictly for bypassing header records which are written directly to the output file. 
		///   If there is a header record requiring additional processing, specify its position within the file.
		///
		/// </summary>
		public int? HeaderRecord { get; set; }
		public int? HeaderRecordFields { get; set; }
		public int? HeaderRecordLength { get; set; }
		public int? FirstRecord { get; set; } = 2;

		/// <summary>
		///
		///  Total number of records to process. Use 0 to indicate all.
		///
		/// </summary>
		public int? RecordsToProcess { get; set; } = 0;

		/// <summary>
		///
		///   specify total number of fields for output file:
		///   ( used for processing comma/tab delimited fields )
		///
		/// </summary>
		public int? FieldsPerRecord { get; set; } = 9;

		/// <summary>
		///
		///   Specify whether output data should be double-quoted.
		///   ( used for processing comma/tab delimited fields )
		///   
		///   "Yes" for yes "No" for no
		///
		/// </summary>
		public string OutputQuotedData { get; set; } = "No";

		/// <summary>
		///
		///   Force formatting of foreign, invalid or not correctable
		///   default is to format only valid or corrected records
		///   applies to batch api with output format guide &gt;= 3 &amp; &lt;= 8
		///   
		///   "Yes" for yes "No" for no
		///
		/// </summary>
		public string FormatAllInputRecords { get; set; } = "No";

		/// <summary>
		///
		///   Specify Postal Code fixed at 6 or 7 characters.
		///   Or specify 'D' for original default behaviour.
		///  
		/// </summary>
		public string PostalCodeFormatGuide { get; set; } = "D";

		/// <summary>
		///
		///   For missing input country codes specify a default value
		///   
		/// </summary>
		public string DefaultCountryCode { get; set; } = "CAN";

		/// <summary>
		///
		///   To force processing of input records according to default
		///   country code enable "OverRideInputCountryCode" 
		///
		/// </summary>
		public string OverRideInputCountryCode { get; set; } = "YES";


		/// <summary>
		///
		///   For missing input language code specify a default value
		///   
		///   I=INPUT, E=ENGLISH, F=FRENCH, C=CPC SPECIFICATION 
		///   
		/// </summary>
		public string DefaultLanguageCode { get; set; } = "I";

		/// <summary>
		///
		///   To force processing of input records according to default
		///   language code enable "OverRideInputLanguageCode" 
		///
		/// </summary>
		public string OverRideInputLanguageCode { get; set; } = "YES";

		/// <summary>
		///
		/// Fixed length fields are defined with pairs of parameters consisting of
		/// an offset from the first character in the record and a field length.
		/// 
		/// Comma/tab delimited records are defined by simply identifying the fields
		/// and the order in which they occur in the record. The offset parameter is used,
		/// but here its meaning is the ordinal position of the field in the record.
		///
		/// The following fields may be defined:
		/// * unique key
		/// * addressee
		/// * street number
		/// * street name
		/// * unit number
		/// * address line
		/// * city
		/// * province/state 
		/// * postal/zip code
		/// * country 
		///
		/// You may define two sets of fields, Input* and Output*.
		///   
		/// If only Input* fields are defined Output* = Input*.
		///
		/// </summary>
		public int? InputKeyOffset { get; set; }
		public int? InputKeyLength { get; set; }
		public int? InputLanguageOffset { get; set; }
		public int? InputLanguageLength { get; set; }
		public int? InputRecipientOffset { get; set; }
		public int? InputRecipientLength { get; set; }
		public int? InputStreetNumberOffset { get; set; }
		public int? InputStreetNumberLength { get; set; }
		public int? InputStreetNameOffset { get; set; }
		public int? InputStreetNameLength { get; set; }
		public int? InputUnitNumberOffset { get; set; }
		public int? InputUnitNumberLength { get; set; }
		public int? InputAddressLineOffset { get; set; }
		public int? InputAddressLineLength { get; set; }
		public int? InputCityOffset { get; set; }
		public int? InputCityLength { get; set; }
		public int? InputProvinceStateOffset { get; set; }
		public int? InputProvinceStateLength { get; set; }
		public int? InputPostalZipCodeOffset { get; set; }
		public int? InputPostalZipCodeLength { get; set; }
		public int? InputCountryOffset { get; set; }
		public int? InputCountryLength { get; set; }


		/// <summary>
		///   comma/tab delimited records are defined by simply identifying the fields
		///   and the order in which they occur in the record. The offset parameter is used,
		///   but here its meaning is the ordinal position of the field in the record.
		///
		///   The following fields may be defined:
		/// * unique key
		/// * addressee,
		/// * street number
		/// * street name
		/// * unit number
		/// * address line,
		/// * city
		/// * province/state 
		/// * postal/zip code
		/// * country 
		///
		///  You may define two sets of fields, Input* and Output*.
		///   
		///  If only Input* fields are defined Output* = Input*.
		///
		///  Note:  *StreetNumber, *StreetName, and *UnitNumber are not available for USA
		///
		///  Note:
		///  
		///  It is possible to have different record formats for CAN and USA addresses
		///  by using country specific keywords of the format CAN_Input* / USA_Input*
		///  and, if necessary, CAN_Output* /USA_Output* keywords.
		///
		/// </summary>
	}


}
