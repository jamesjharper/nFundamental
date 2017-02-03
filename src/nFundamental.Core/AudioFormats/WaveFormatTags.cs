namespace Fundamental.Core.AudioFormats
{
    /// <summary>
    /// Wave format tags 
    /// </summary>
    public enum WaveFormatTag : ushort
    {
        Unknown = 0x0000,

        /// <summary> PCM </summary>
        Pcm = 0x0001,

        /// <summary> ADPCM </summary>
        Adpcm = 0x0002,

        /// <summary> IEEE Float </summary>
        IeeeFloat = 0x0003,

        /// <summary> Compaq Computer Corp - VSELP.</summary>
        Vselp = 0x0004,

        /// <summary> IBM Corporation - CVSD</summary>
        IbmCvsd = 0x0005,

        /// <summary> ALAW </summary>
        ALaw = 0x0006,

        /// <summary> MULAW </summary>
        MuLaw = 0x0007,

        /// <summary> DTS </summary>
        Dts = 0x0008,

        /// <summary> DRM </summary>
        Drm = 0x0009,

        /// <summary> OKI - ADPCM</summary>
        OkiAdpcm = 0x0010,

        /// <summary> Intel Corporation - DVI ADPCM</summary>
        DviAdpcm = 0x0011,

        /// <summary> Intel Corporation - IMA ADPCM </summary>
        ImaAdpcm = DviAdpcm,

        /// <summary> Videologic - Mediaspace ADPCM</summary>
        MediaspaceAdpcm = 0x0012,

        /// <summary> Sierra Semiconductor Corp - ADPCM</summary>
        SierraAdpcm = 0x0013,

        /// <summary> Antex Electronics Corporation - G723 ADPCM </summary>
        AntexG723Adpcm = 0x0014,

        /// <summary> DSP Solutions Inc - DigiStd </summary>
        DspSolutionsDigiStd = 0x0015,

        /// <summary> DSP Solutions Inc - DigiFix</summary>
        DspSolutionsDigiFix = 0x0016,

        /// <summary> Dialogic Corporation - OKI ADPCM -</summary>
        DialogicOkiAdpcm = 0x0017,

        /// <summary> Media Vision Inc - ADPCM </summary>
        MediaVisionAdpcm = 0x0018,

        /// <summary> Hewlett-Packard Company - CU Codec </summary>
        CuCodec = 0x0019,

        /// <summary> Yamaha Corporation of America - ADPCM </summary>
        YamahaAdpcm = 0x0020,

        /// <summary>Speech Compression - SonarC </summary>
        SonarC = 0x0021,

        /// <summary> DSP Group Inc - TrueSpeech </summary>
        DspGroupTrueSpeech = 0x0022,

        /// <summary> Echo Speech Corporation - EchoSpeech </summary>
        EchoSpeechCorporation1 = 0x0023,

        /// <summary> Virtual Music Inc - Audio File AF36 </summary>
        AudioFileAf36 = 0x0024,

        /// <summary> Audio Processing Technology - APTX </summary>
        Aptx = 0x0025,

        /// <summary> Virtual Music Inc - Audio File AF10 </summary>
        AudioFileAf10 = 0x0026,

        /// <summary>Aculab PLC - Prosody 1612 </summary>
        Prosody1612 = 0x0027,

        /// <summary>Merging Technologies S.A. - LRC </summary>
        Lrc = 0x0028,

        /// <summary> Dolby Laboratories - Dolby AC2 </summary>
		DolbyAc2 = 0x0030,

        /// <summary> GSM610 </summary>
        Gsm610 = 0x0031,

        /// <summary>Microsoft Corporation - MSN Audio </summary>
		MsnAudio = 0x0032,

        /// <summary> Antex Electronics Corporation - ADPCME </summary>
		AntexAdpcme = 0x0033,

        /// <summary> Control Resources Limited - Control Resources VQLPC </summary>
		ControlResVqlpc = 0x0034,

        /// <summary> DSP Solutions Inc - Digi Real </summary>
		DigiReal = 0x0035,

        /// <summary> DSP Solutions Inc - Digi ADPCM </summary>
		DigiAdpcm = 0x0036,

        /// <summary> Control Resources Limited - Control Resources CR10</summary>
		ControlResCr10 = 0x0037,

        /// <summary>Natural MicroSystems - VBX ADPCM </summary>
        NmsVbxadpcm = 0x0038,

        /// <summary> Crystal Semiconductor - IMA ADPCM </summary>
        CsImaadpcm = 0x0039,

        /// <summary> Echo Speech Corporation - Echo SC3 </summary>
        Echosc3 = 0x003A,

        /// <summary> Rockwell International - Rockwell ADPCM </summary>
        RockwellAdpcm = 0x003B,

        /// <summary>Rockwell International - Rockwell DigitalK </summary>
        RockwellDigitalk = 0x003C,

        /// <summary> Xebec Multimedia Solutions Limited - Xebec </summary>
        Xebec = 0x003D,

        /// <summary> Antex Electronics Corporation - G721 ADPCM </summary>
        AntexG721Adpcm = 0x0040,

        /// <summary> Antex Electronics Corporation - G728 CELP </summary>
        AntexG728Celp = 0x0041,

        /// <summary> Microsoft Corporation - Msg723 </summary>
        Msg723 = 0x0042,
       
        /// <summary> MPEG </summary>
        Mpeg = 0x0050,

        /// <summary> InSoft Inc - RT24 </summary>
        InSoftRt24 = 0x0052,

        /// <summary> InSoft Inc - PAC </summary>
        InSoftPac = 0x0053,

        /// <summary> ISO/MPEG Layer 3 </summary>
        MpegLayer3 = 0x0055,

        /// <summary> Lucent Technologies - G723 </summary>
        LucentG723 = 0x0059,

        /// <summary> Cirrus Logic </summary>
        Cirrus = 0x0060,

        /// <summary> ESS Technology - ESPCM </summary>
        Espcm = 0x0061,

        /// <summary> Voxware Inc </summary>
        Voxware = 0x0062,

        /// <summary> Canopus, co. Ltd - Atrac </summary>
        CanopusAtrac = 0x0063,

        /// <summary> APICOM - G726 ADPCM </summary>
        ApiComG726Adpcm = 0x0064,

        /// <summary> APICOM - G722 ADPCM </summary>
        ApiComG722Adpcm = 0x0065,

        /// <summary> DSAT Display </summary>
        DsatDisplay = 0x0067,

        /// <summary> Voxware Inc - Byte Aligned </summary>
        VoxwareByteAligned = 0x0069,

        /// <summary> Voxware Inc - AC8 </summary>
        VoxwareAc8 = 0x0070,

        /// <summary> Voxware Inc - AC10 </summary>
        VoxwareAc10 = 0x0071,

        /// <summary> Voxware Inc - AC16 </summary>
        VoxwareAc16 = 0x0072, 

        /// <summary> Voxware Inc - AC20 </summary>
        VoxwareAc20 = 0x0073,         

        /// <summary> Voxware Inc - RT24 </summary>
        VoxwareRt24 = 0x0074,

        /// <summary> Voxware Inc - RT29 </summary>
        VoxwareRt29 = 0x0075,

        /// <summary> Voxware Inc - RT24HW </summary>
        VoxwareRt29Hw = 0x0076,

        /// <summary> Voxware Inc - VR12 </summary>
        VoxwareVr12 = 0x0077,

        /// <summary> Voxware Inc - VR18 </summary>
        VoxwareVr18 = 0x0078,

        /// <summary> Voxware Inc - TQ40 </summary>
        VoxwareTq40 = 0x0079,

        /// <summary> Softsound Ltd - Softsound </summary>
        Softsound = 0x0080,

        /// <summary> Voxware Inc - TQ60 </summary>
        VoxwareTq60 = 0x0081,

        /// <summary> Microsoft Corporation - MS RT24 </summary>
        Msrt24 = 0x0082,

        /// <summary> AT and T Labs Inc - G729A </summary>
        G729A = 0x0083,

        /// <summary> Motion Pixels - Mvi Mvi 2  </summary>
        MviMvi2 = 0x0084,

        /// <summary> DataFusion Systems PTY LTD - DF G726 </summary>
        DfG726 = 0x0085,

        /// <summary> DataFusion Systems PTY LTD - DF GSM 610 </summary>
        DfGsm610 = 0x0086,

        /// <summary> Iterated Systems Inc - ISI Audio </summary>
        Isiaudio = 0x0088,

        /// <summary> OnLive! Technologies Inc </summary>
        Onlive = 0x0089,

        /// <summary>Siemens Business Communications Sys - SBC 24</summary>
        Sbc24 = 0x0091,

        /// <summary> Sonic Foundry - Dolby AC3 SPDIF </summary>
        DolbyAc3Spdif = 0x0092,

        /// <summary> MediaSonic - G723 </summary>
        /// <summary></summary>
        MediasonicG723 = 0x0093,

        /// <summary>Aculab PLC -  Prosody 8Kbps</summary>
        Prosody8Kbps = 0x0094,

        /// <summary> ZyXEL Communications Inc - ADPCM </summary>
        ZyxelAdpcm = 0x0097,

        /// <summary> Philips Speech Processing - LPCBB</summary>
        PhilipsLpcbb = 0x0098,

        /// <summary>Studer Professional Audio AG - Packed</summary>
        StuderPacked = 0x0099,

        /// <summary>Malden Electronics LTD - Phonytalk</summary>
        MaldenPhonytalk = 0x00A0,

        /// <summary> GSM </summary>
		Gsm = 0x00A1,

        /// <summary> G729 </summary>
        G729 = 0x00A2,

        /// <summary> G723 </summary>
        G723 = 0x00A3,

        /// <summary> ACELP </summary>
		Acelp = 0x00A4,

        /// <summary> Rhetorex Inc - ADPCM </summary>
        RhetorexAdpcm = 0x0100,

        /// <summary> BeCubed Software Inc - IRAT </summary>
        BeCubedIrat = 0x0101,

        /// <summary> Vivo Software - G723</summary>
        VivoG723 = 0x0111,

        /// <summary> Vivo Software - Siren </summary>
        VivoSiren = 0x0112,

        /// <summary> Digital Equipment Corporation - G723 </summary>
        DigitalG723 = 0x0123,

        /// <summary> Sanyo Electric CO LTD - LD ADPCM </summary>
        SanyoLdAdpcm = 0x0125,

        /// <summary> Sipro Lab Telecom Inc - ACEPL net </summary>
        SiprolabAceplnet = 0x0130,

        /// <summary> Sipro Lab Telecom Inc - ACEPL 4800 </summary>
        SiprolabAcelp4800 = 0x0131,

        /// <summary> Sipro Lab Telecom Inc - ACEPL 8V3 </summary>
        SiprolabAcelp8V3 = 0x0132,

        /// <summary> Sipro Lab Telecom Inc - ACEPL G729 </summary>
        SiprolabG729 = 0x0133,

        /// <summary> Sipro Lab Telecom Inc - ACEPL G729A </summary>
        SiprolabG729A = 0x0134,

        /// <summary> Sipro Lab Telecom Inc - Kelvin </summary>
        SiprolabKelvin = 0x0135,

        /// <summary> Dictaphone Corporation - G726 ADPCM </summary>
        DictaphoneG726Adpcm = 0x0140,

        /// <summary> Qualcomm Inc - Purevoice </summary>
        QualcommPurevoice = 0x0150,

        /// <summary> Qualcomm Inc - Halfrate </summary>
        QualcommHalfrate = 0x0151,

        /// <summary> Ring Zero Systems Inc - TUBGSM </summary>
        Tubgsm = 0x0155,

        /// <summary> Microsoft Corporation - MS Audio </summary>
        Msaudio1 = 0x0160,

        /// <summary> Microsoft Corporation - WM Audio 2 </summary>
        Wmaudio2 = 0x0161,

        /// <summary> Microsoft Corporation - WM Audio 3 </summary>
        Wmaudio3 = 0x0162,

        /// <summary> Unisys Corporation - NAP ADPCM </summary>
		UnisysNapAdpcm = 0x0170,

        /// <summary> Unisys Corporation - NAP ULAW </summary>
        UnisysNapUlaw = 0x0171,

        /// <summary> Unisys Corporation - NAP ALAW </summary>
        UnisysNapAlaw = 0x0172,

        /// <summary> Unisys Corporation - NAP 16K </summary>
        UnisysNap16K = 0x0173,

        /// <summary> Creative Labs Inc - ADPCM </summary>
        CreativeAdpcm = 0x0200,

        /// <summary> Creative Labs Inc - Fastspeech 8 </summary>
        CreativeFastspeech8 = 0x0202,

        /// <summary> Creative Labs Inc - Fastspeech 10 </summary>
        CreativeFastspeech10 = 0x0203,

        /// <summary> UHER informatic GmbH - ADPCM </summary>
        UherAdpcm = 0x0210,

        /// <summary> Quarterdeck Corporation - Quarterdeck </summary>
        Quarterdeck = 0x0220,

        /// <summary> I-link Worldwide - VC </summary>
        IlinkVc = 0x0230,

        /// <summary> Aureal Semiconductor - Raw Sport </summary>
        AurealRawSport = 0x0240,

        /// <summary> ESS Technology Inc - AC3 </summary>
        EsstAc3 = 0x0241,

        /// <summary>Interactive Products Inc - HSX </summary>
        IpiHsx = 0x0250,

        /// <summary>Interactive Products Inc - RPELP </summary>
        IpiRpelp = 0x0251,

        /// <summary> Consistent Software - 2</summary>
        Cs2 = 0x0260,

        /// <summary> Sony Corporation - SCX </summary>
        SonyScx = 0x0270,

        /// <summary> Fujitsu Corporation - Fm Towns Snd </summary>
        FujitsuFmTownsSnd = 0x0300,

        /// <summary> Brooktree Corporation  - BtvDigital </summary>
        BtvDigital = 0x0400,

        /// <summary> QDesign Corporation - Music</summary>
        QdesignMusic = 0x0450,

        /// <summary> AT and T Labs Inc - VME VMPCM </summary>
        ATTVmeVmpcm = 0x0680,

        /// <summary> AT and T Labs Inc - TPC </summary>
        ATTTpc = 0x0681,

        /// <summary> Ing C. Olivetti and C., S.p.A. - Oli GSM </summary>
        OliGsm = 0x1000,

        /// <summary> Ing C. Olivetti and C., S.p.A. - ADPCM </summary>
        OliAdpcm = 0x1001,

        /// <summary> Ing C. Olivetti and C., S.p.A. - CELP </summary>
        OliCelp = 0x1002,

        /// <summary> Ing C. Olivetti and C., S.p.A. - SBC </summary>
        OliSbc = 0x1003,

        /// <summary> Ing C. Olivetti and C., S.p.A. - OPR </summary>
        OliOpr = 0x1004,

        /// <summary>  Lernout and Hauspie - LH Codec </summary>
        LhCodec = 0x1100,

        /// <summary> Norris Communications Inc. Norris </summary>
        Norris = 0x1400,

        /// <summary> AT and T Labs Inc -  Soundspace Musicompress </summary>
        ATTSoundspaceMusicompress = 0x1500,

        /// <summary> FAST Multimedia AG  - DVM </summary>
        FastDvm = 0x2000, 


        // others - not from MS headers
        /// <summary> VORBIS1 </summary>
        Vorbis1 = 0x674f,


        /// <summary> VORBIS2 </summary>
        Vorbis2 = 0x6750,

        /// <summary> VORBIS3 </summary>
        Vorbis3 = 0x6751,

        /// <summary> VORBIS1P </summary>
        Vorbis1P = 0x676f,

        /// <summary> VORBIS2P </summary>
        Vorbis2P = 0x6770,

        /// <summary> VORBIS3P </summary>
        Vorbis3P = 0x6771,

        /// <summary> Extensible </summary>
        Extensible = 0xFFFE,

        /// <summary> Development </summary>
        Development = 0xFFFF,

    }
}
