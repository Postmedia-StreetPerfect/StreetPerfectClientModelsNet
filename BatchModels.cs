using Amazon.ECS.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;

namespace StreetPerfect.Models
{

    public class BatchUploadRequest
    {
        /// <summary>
        /// This is just a regular unicode string that your json framework will encode as utf8
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Specify the encoding of the EncodedData (see batch/encodings for a list of possible encodings to use)
        /// 
        /// Note that iso-8859-1 is best for this.
        /// </summary>
        public string Encoding { get; set; }
    }

    public class BatchUploadForm
    {
        /// <summary>
        /// file a form-data file input 
        /// </summary>
        public IFormFile file { get; set; }

        /// <summary>
        /// Specify the encoding of the file (see batch/encodings for a list of possible encodings to use)
        /// 
        /// Note that data must be ultimately iso-8859-1 compatible, but UTF8 is assumed if not set.
        /// </summary>
        public string encoding { get; set; }

        /// <summary>
        /// If file is zipped you must set this true
        /// </summary>
        public bool? is_zipped { get; set; }
    }

    public class BatchEncoding
    {
        public string Encoding { get; set; }
        public int CodePage { get; set; }
    }

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

    public class BatchRunInfo
    {
        public int exitCode { get; set; }
        public string exitStatus { get; set; }
        public string exitMsg { get; set; }
        public int runTimeSecs { get; set; }
        public DateTime runTimeStart { get; set; }
        public int totalProcessed { get; set; }
        public int validAddresses { get; set; }
        public int invalidAddresses { get; set; }
        public int questionableLvrAddresses { get; set; }
        public int questionableRuralAddresses { get; set; }
        public int questionableApartmentAddresses { get; set; }
    }

    public class BatchStatus
    {
        public enum StatusType { Unknown, Error, Empty, Starting, Running, Stopping, Stopped, InputReady, OutputReady }
        public string Status { get; set; }
        public DateTime? StartTimeUtc { get; set; }
        public string Msg { get; set; }

        /// <summary>
        /// info is only accessible when status is OutputReady
        /// </summary>
        public BatchRunInfo RunInfo { get; set; }
    }


    public class BatchConfig
    {
        [JsonIgnore]
        public string SPDatabasePath { get; set; }

        [JsonIgnore]
        public string SPIOPath { get; set; }

        [JsonInclude]
        public string DatabaseOptions { get; set; }

        /// <summary>
        /// Defaults to 'SUITE'
        /// 
        /// | English Abbr. | French Abbr. | English Full Name | French Full Name |
        /// |---------------|--------------|-------------------|------------------|
        /// | ‘UNIT’        | ‘UNITE’      | ‘UNIT’            | ‘UNITE’          |
        /// | ‘APT’         | ‘APP’        | ‘APARTMENT’       | ‘APPARTMENT’ |
        /// | ‘SUITE’       | ‘BUREAU’     | ‘SUITE’           | ‘BUREAU’ |
        /// | ‘TH’          |              | ‘TOWNHOUSE’ |
        /// | ‘TWNHSE’      |              | ‘TOWNHOUSE’ |
        /// | ‘RM’          |              | ‘ROOM’  |
        /// | ‘PH’          |              | ‘PENTHOUSE’  |
        /// | ‘PIECE’       |              |                   | ‘PIECE’  |
        /// | ‘SALLE’       |              |                   | ‘SALLE’ |
        /// </summary>
        /// <example>SUITE</example>
        public string PreferredUnitDesignatorKeyword { get; set; }

        /// <summary>
        /// Defaults to 'K'
        /// 
        /// 'K' Keyword Style
        /// 
        /// e.g. 123 MAIN ST SUITE 5
        /// 
        /// 'W' Western Style
        /// 
        /// e.g. 5-123 MAIN ST
        /// </summary>
        /// <example>K</example>
        public string PreferredUnitDesignatorStyle { get; set; }

        /// <summary>
        /// Defaults to 'N'
        /// 
        /// 'N' - Natural
        /// Follows the language rules:
        /// - Street type placement relative to street name.
        /// - English types always follow street names.
        /// - Upper case
        ///  
        /// 'S' - Street
        /// - Forces the street name to always appear first
        /// - Upper case
        /// 
        /// Alternate values
        /// 1. Same as N +
        /// 2. Same as S +
        /// 3. Same as N with upper case French accents +
        /// 4. Same as S with upper case French accents +
        /// 5. Same as N with mixed case +
        /// 6. Same as S with mixed case +
        /// 7. Same as N with mixed case French accents +
        /// 8. Same as S with mixed case French accents +
        /// 
        /// ( + Only accessible from the Format function )
        /// </summary>
        /// <example>N</example>
        public string OutputFormatGuide { get; set; }

        /// <summary>
        /// Defaults to 'D'
        /// 
        /// 'D' Detail Report - Print detail messages.
        /// 
        /// 'S' Summary Report - Print summary only.
        /// 
        /// 'E' Error Report - Print errors only.
        /// 
        /// 'N' No Report
        /// </summary>
        /// <example>D</example>
        public string ExceptionReportLevel { get; set; }

        /// <summary>
        /// Controls what type of message codes are returned with the correction messages.
        /// 
        /// Defaults to 'N'
        /// 
        /// 'Y' - returns a 3 digit prefix to address correction messages for
        /// programmatic use. These may be used to provide a filtering
        /// capability. Message codes can be found in the DocumentFiles
        /// installation folder.
        /// 
        /// 'N' - returns a 3 character prefix to address correction messages
        /// indicating message class. For Canadian product only, these
        /// prefixes are:
        /// 
        /// INP - Original input line
        /// 
        /// INF - Informational message
        /// 
        /// OPT - Optimization message
        /// 
        /// CHG - Change message
        /// 
        /// TRY - Try message - engine has identified one or more possibilities as a potential correction but there was insufficient data or ambiguous results making identification of a correction unreliable.
        /// 
        /// ERR - Error message
        /// | Value | Print Message Class | Return Message Class | Print Message Numbers | Return Message Numbers |
        /// |-------|---------------------|----------------------|-----------------------|------------------------|
        /// |  N/0  |         N           |           N          |           N           |            N           | 
        /// |  Y/1  |         N           |           N          |           Y           |            N           | 
        /// |    2  |         N           |           N          |           N           |            Y           | 
        /// |    3  |         N           |           N          |           Y           |            Y           | 
        /// |    4  |         N           |           Y          |           N           |            Y           | 
        /// |    5  |         N           |           Y          |           Y           |            Y           | 
        /// |    6  |         Y           |           Y          |           Y           |            Y           | 
        /// </summary>
        /// <example>N</example>
        public string PrintMessageNumbers { get; set; } = "N";

        /// <summary>
        /// Defaults to 'false'
        /// 
        /// 'true' - Yes, print information messages in exception report.
        /// 
        /// This includes items from the Canada Post LVR (Text 4) file.
        /// 
        /// 'false' - No, do not print information messages.
        /// </summary>
        /// <example>true</example>
        public bool? PrintInformationMessages { get; set; }

        /// <summary>
        /// Defaults to 'true'
        /// 
        /// 'true' - print change messages in exception report.
        /// - This will identify changes between the old and corrected address.
        /// 
        /// 'false' - do not print change messages.
        /// </summary>
        /// <example>true</example>
        public bool? PrintChangeMessages { get; set; }

        /// <summary>
        /// Defaults to 'true'
        /// 
        /// 'true' - print error messages in exception report.
        /// - This will document specific address errors.
        /// 
        /// 'false' - do not print error messages.
        /// </summary>
        /// <example>true</example>
        public bool? PrintErrorMessages { get; set; }

        /// <summary>
        /// Defaults to 'true'
        /// 
        /// 'true' - print try messages in exception report. This will list the best possible address corrections up to the maximum tries (SB_IN_PRINT_MAX_TRY_MESS)
        /// 
        /// 'false' - do not print try messages.		
        /// </summary>
        /// <example>true</example>
        public bool? PrintTryMessages { get; set; }

        /// <summary>
        /// Defaults to 'true'
        /// 
        /// 'true' - print optimization messages in exception report. This will list optimization performed on the address. This occurs only if Optimum Address Flag (OptimizeAddress) = 'Y'
        /// 
        /// 'false' - do not print optimization messages.
        /// </summary>
        /// <example>true</example>
        public bool? PrintOptimizeMessages { get; set; }


        /// <summary>
        /// 'Y' - replace address components with Canada Post symbols where possible.
        /// 
        /// E.g. 123 KING STREET EAST Input
        /// 
        /// 123 KING ST E Output
        /// 
        /// 'N' - No, do not optimize address to Canada Post symbols.
        /// 
        /// 'S' - Standardize, perform a series of possible changes to the input address including:
        /// 
        /// - convert of all CPC keywords to symbols
        /// - remove Extra Info and Unidentified Components from address line
        /// - change input municipality name to CPC database municipality name
        /// - format unit / apt / suite information according to PreferredUnitDesignatorStyle keyword
        /// - The Correct and Parse APIs will honour the OutputFormatGuide parameter so that it is possible to receive mixed case or French accented data results from these APIs. 
        /// 
        /// </summary>
        /// <example>S</example>
        public string OptimizeAddress { get; set; }


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
        /// <example>N</example>
        public string ProcessErrors { get; set; }

        //public string ReportByCompanyID { get; set; } = "SAMP";
        //public string ReportForCompanyID { get; set; } = "SAMP";
        //public string ReportFileID { get; set; } = "001";

        /// <summary>
        /// Error Tolerance Indicator
        /// - Determines how “closely” an input address must come to a Canada Post address to be considered a match.
        /// - The value indicates the number of components that may be in variance.
        /// - Allowed values; 0 - 4 
        /// </summary>
        /// <example>2</example>
        public int? ErrorTolerance { get; set; }

        /// <summary>
        /// Maximum Tries Flag
        /// - The maximum number of possible alternate addresses to print if unable to correct.
        /// - These alternate addresses will appear on the exception report.
        /// </summary>
        /// <example>5</example>
        public int? MaximumTryMessages { get; set; }

        /// <summary>
        /// ‘Y’ Yes – standard correction processing occurs. Invalid or not correctable rural addresses returned as “I/N”. 
        ///This setting maximizes actual address accuracy but minimizes CPC Certification Accuracy.
        ///
        /// ‘N’ No – If input PC Valid LVR PC and not correctable or was corrected and PC changed return input as V. 
        /// Increment "Questionable" LVR count. This setting minimizes actual address accuracy but maximizes CPC Certification Accuracy.
        ///
        /// ‘Q’ Questionable – If input PC Valid LVR PC and not correctable or was corrected and PC changed return input as V. 
        /// Increment "Questionable" LVR count and output Questionable message. This setting optimizes actual address accuracy and maximizes CPC Certification Accuracy.
        /// </summary>
        /// <example>Q</example>
        public string CorrectLvrAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>Q</example>
        public string CorrectLvrAmbiguity { get; set; }

        /// <summary>
        /// Correct rural addresses
        /// 
        /// ‘Y’ Yes – standard correction processing occurs. Invalid or	not correctable rural addresses returned as “I/N”. This setting maximizes actual address accuracy but minimizes CPC Certification Accuracy.
        /// 
        /// ‘N’ No - If valid input address return input address as V. If valid rural PC input, return input address as V. Increment 
        ///"Questionable" rural count.This setting minimizes actual address accuracy but maximizes CPC Certification Accuracy.
        /// 
        /// ‘Q’ Questionable – Attempt to correct record. If not	correctable but valid rural PC input, return input address as V.Increment 
        ///"Questionable" rural count and output Questionable message. This setting optimizes actual address accuracy and maximizes CPC Certification Accuracy.
        /// </summary>
        /// <example>Q</example>
        public string CorrectRuralAddress { get; set; }

        /// <summary>
        /// Report all unidentified address components
        /// </summary>
        /// <example>true</example>
        public bool? ReportAllUnidentified { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>true</example>
        public bool? ReportOrphanUdkAsExtraInfo { get; set; }


        /// <summary>
        ///
        ///   function to be performed for this run
        ///   "C" for correction, "V" for validation, "P" for parse
        ///
        /// </summary>
        /// <example>C</example>
        public string Function { get; set; }

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
        /// </summary>
        /// <example>true</example>
        public bool? OutputStatusFlag { get; set; }

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
        /// <example>C</example>
        public string FileFormat { get; set; }

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
        /// <example>L</example>
        public string AddressFormat { get; set; }

        /// <summary>
        ///
        ///   First record in the file to start processing
        ///   
        ///   Note: This is strictly for bypassing header records which are written directly to the output file. 
        ///   If there is a header record requiring additional processing, specify its position within the file.
        ///
        ///	 (one's based)
        /// </summary>
        /// <example></example>
        public int? HeaderRecord { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example></example>
        /// <example></example>
        public int? HeaderRecordFields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example></example>
        /// <example></example>
        public int? HeaderRecordLength { get; set; }

        /// <summary>
        /// Record to start processing - one's based.
        /// </summary>
        /// <example></example>
        /// <example>2</example>
        public int? FirstRecord { get; set; }

        /// <summary>
        ///
        ///  Total number of records to process. Use 0 to indicate all.
        ///
        /// </summary>
        /// <example>0</example>
        public int? RecordsToProcess { get; set; }

        /// <summary>
        ///
        ///   Specify total number of fields for OUTPUT file.
        ///   
        ///   ( used for processing comma/tab delimited fields)
        ///
        /// </summary>
        /// <example>0</example>
        public int? FieldsPerRecord { get; set; }

        /// <summary>
        ///
        ///   Specify whether output data should be double-quoted.
        ///   ( used for processing comma/tab delimited fields )
        ///   
        ///   "Yes" for yes "No" for no
        ///
        /// </summary>
        /// <example>false</example>
        public bool? OutputQuotedData { get; set; }

        /// <summary>
        ///
        ///   Force formatting of foreign, invalid or not correctable
        ///   
        ///   Default is to format only valid or corrected records
        ///   
        ///   Applies to batch api with output format guide &gt;= 3 &amp; &lt;= 8
        ///   
        /// </summary>
        /// <example>false</example>
        public bool? FormatAllInputRecords { get; set; }

        /// <summary>
        ///
        ///   Specify Postal Code fixed at 6 or 7 characters.
        ///   
        ///   Or specify 'D' for original default behaviour.
        ///  
        /// </summary>
        /// <example>D</example>
        public string PostalCodeFormatGuide { get; set; }

        /// <summary>
        ///
        ///   For missing input country codes specify a default value
        ///   
        /// </summary>
        /// <example>CAN</example>
        public string DefaultCountryCode { get; set; }

        /// <summary>
        ///
        ///   To force processing of input records according to default
        ///   country code enable "OverRideInputCountryCode" 
        ///
        /// </summary>
        /// <example>true</example>
        public bool? OverRideInputCountryCode { get; set; }


        /// <summary>
        ///
        ///   For missing input language code specify a default value
        ///   
        ///   I=INPUT, E=ENGLISH, F=FRENCH, C=CPC SPECIFICATION 
        ///   
        /// </summary>
        /// <example>I</example>
        public string DefaultLanguageCode { get; set; }

        /// <summary>
        ///
        ///   To force processing of input records according to default
        ///   language code enable "OverRideInputLanguageCode" 
        ///
        /// </summary>
        /// <example>true</example>
        public bool? OverRideInputLanguageCode { get; set; }

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
        /// the example values are setup for a comma delimited data file with a heading: key,addressee,address,city,province,postal_code
        /// </summary>
        /// <example>1</example>
        public int? InputKeyOffset { get; set; }
        public int? InputKeyLength { get; set; }
        public int? InputLanguageOffset { get; set; }
        public int? InputLanguageLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>2</example>
        public int? InputRecipientOffset { get; set; }
        public int? InputRecipientLength { get; set; }
        public int? InputStreetNumberOffset { get; set; }
        public int? InputStreetNumberLength { get; set; }
        public int? InputStreetNameOffset { get; set; }
        public int? InputStreetNameLength { get; set; }
        public int? InputUnitNumberOffset { get; set; }
        public int? InputUnitNumberLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>3</example>
        public int? InputAddressLineOffset { get; set; }
        public int? InputAddressLineLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>4</example>
        public int? InputCityOffset { get; set; }
        public int? InputCityLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>5</example>
        public int? InputProvinceStateOffset { get; set; }
        public int? InputProvinceStateLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>6</example>
        public int? InputPostalZipCodeOffset { get; set; }
        public int? InputPostalZipCodeLength { get; set; }
        public int? InputCountryOffset { get; set; }
        public int? InputCountryLength { get; set; }

    }


}
