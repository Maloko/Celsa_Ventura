USE [SBO_LUVEGI_SEGUNDO]
GO
/****** Object:  StoredProcedure [dbo].[VS_FINANB1_DESTINOS_POR_PERIODO_DTW]    Script Date: 20/12/2020 16:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[VS_FINANB1_DESTINOS_POR_PERIODO_DTW](
@PERIODENTRY INT
)
--WITH ENCRYPTION
AS
BEGIN
	
	DECLARE @FECHAINI DATETIME 
	DECLARE @FECHAFIN DATETIME

	SELECT @FECHAINI=F_RefDate,@FECHAFIN=T_RefDate FROM OFPR WHERE AbsEntry = @PERIODENTRY

	DECLARE @CTAIMPUTABLE nvarchar(30)
	SET @CTAIMPUTABLE = (SELECT TOP 1 AcctCode FROM OACT WHERE U_BPP_PCTC = 'I' AND Postable = 'Y')

	DECLARE @DIGNATURALEZA CHAR(1)
	SET @DIGNATURALEZA = (SELECT TOP 1 U_CNATU FROM [@VS_CONFIG_FB1])



	DECLARE @REST TABLE(
	astOrigen int, 
	Cuentas nvarchar(30), 
	deb numeric(19,6),
	cred numeric(19,6),
	fdeb numeric(19,6),
	fcred numeric(19,6),
	fcurr nvarchar(6), 
	fechaCont nvarchar(10), 
	fechaDocu nvarchar(10), 
	fechaVenc nvarchar(10), 
	referencia nvarchar(200), 
	glosa nvarchar(200),
	OCR1 nvarchar(60),
	OCR2 nvarchar(60),
	OCR3 nvarchar(60),
	OCR4 nvarchar(60),
	OCR5 nvarchar(60),
	Proyecto nvarchar(10)
	)


		INSERT INTO @REST
		SELECT
			'astOrigen'			=	T0.TransId
			,'Cuentas'		    =	CASE WHEN ISNULL(T0.U_BPP_CTAD,'')='' THEN
									    CASE WHEN T0.TransType='18' THEN
											ISNULL((SELECT TOP 1 TX.U_BPP_CTAD FROM PCH1 TX INNER JOIN OPCH TX1 ON TX1.DOCENTRY=TX.DOCENTRY AND TX1.TransId=T0.TransId AND TX.AcctCode=T0.Account),T1.U_VS_CTDS)
										ELSE
											ISNULL(T1.U_VS_CTDS,'')
										END
									ELSE
										T0.U_BPP_CTAD
									END
			,'deb'              =   CASE ISNULL(T0.Debit,0)
									WHEN 0 THEN T0.SYSDeb 
									ELSE
									T0.Debit
									END
			,'cred'             =   CASE ISNULL(T0.Credit,0)
									WHEN 0 THEN T0.SYSCred 
									ELSE
									T0.Credit
									END
			,'fdeb'             =   ISNULL(T0.FCDebit,0) 
			,'fcred'		    =   ISNULL(T0.FCCredit,0) 
			,'fcurr'		    =   ISNULL(T0.FCCurrency,'') 
			,'fechaCont'		=	FORMAT(T0.RefDate,'yyyyMMdd')
			,'fechaDocu'		=	FORMAT(T0.TaxDate,'yyyyMMdd')
			,'fechaVenc'		=	FORMAT(T0.DueDate,'yyyyMMdd') 
			,'referencia'		=	CASE CONVERT(NVARCHAR,T0.TransType) 
										WHEN '46' THEN 'PP'
										WHEN '321' THEN 'ID' 
										WHEN '24' THEN 'PR'
										WHEN '30' THEN 'AS'
										WHEN '18' THEN 'TT'
										WHEN '19' THEN 'AC'
										WHEN '59' THEN 'EM'
										WHEN '60' THEN 'SM'
										WHEN '69' THEN 'DI'
										WHEN '13' THEN 'RF'
										ELSE CONVERT(NVARCHAR,T0.TransType) END+' '+CONVERT(NVARCHAR,T0.BaseRef)
			,'glosa'			=	T0.LineMemo---UPPER(REPLACE(REPLACE(T0.LineMemo,'ó','o'),'ú','u'))
			,'OCR1'				=	ISNULL(T0.ProfitCode,'')
			,'OCR2'				=	ISNULL(T0.OcrCode2,'')
			,'OCR3'				=	ISNULL(T0.OcrCode3,'')
			,'OCR4'				=	ISNULL(T0.OcrCode4,'')
			,'OCR5'				=	ISNULL(T0.OcrCode5,'')
			,'Proyecto'	        =   ISNULL(T1.Project,'')
		FROM 
			JDT1 T0 
			INNER JOIN OACT T1 ON T1.AcctCode = T0.Account AND LEFT(T1.FormatCode,1) = @DIGNATURALEZA AND T1.U_BPP_PCTC='N'
		WHERE 
			T0.RefDate>=@FECHAINI and T0.RefDate<=@FECHAFIN AND T0.U_BPP_DCPR='N'
			AND (((ISNULL(T0.Debit,0) - ISNULL(T0.Credit,0)) <> 0) OR ((ISNULL(T0.FCDebit,0) - ISNULL(T0.FCCredit,0)) <> 0) OR ((ISNULL(T0.SYSDeb,0) - ISNULL(T0.SYSCred,0)) <> 0))
		

		
		INSERT INTO @REST
		SELECT
			'astOrigen'			=	T0.TransId
			,'Cuentas '		    =	ISNULL(T1.U_BPP_PCCR,@CTAIMPUTABLE)		
			,'deb'              =   CASE ISNULL(T0.Credit,0)
									WHEN 0 THEN T0.SYSCred 
									ELSE
									T0.Credit
									END 
			,'cred'             =   CASE ISNULL(T0.Debit,0)
									WHEN 0 THEN T0.SYSDeb 
									ELSE
									T0.Debit
									END
			,'fdeb'             =   ISNULL(T0.FCCredit,0) 
			,'fcred'		    =   ISNULL(T0.FCDebit,0) 
			,'fcurr'		    =   ISNULL(T0.FCCurrency,'') 
			,'fechaCont'		=	FORMAT(T0.RefDate,'yyyyMMdd')
			,'fechaDocu'		=	FORMAT(T0.TaxDate,'yyyyMMdd')
			,'fechaVenc'		=	FORMAT(T0.DueDate,'yyyyMMdd') 
			,'referencia'		=	CASE CONVERT(NVARCHAR,T0.TransType) 
										WHEN '46' THEN 'PP'
										WHEN '321' THEN 'ID' 
										WHEN '24' THEN 'PR'
										WHEN '30' THEN 'AS'
										WHEN '18' THEN 'TT'
										WHEN '19' THEN 'AC'
										WHEN '59' THEN 'EM'
										WHEN '60' THEN 'SM'
										WHEN '69' THEN 'DI'
										WHEN '13' THEN 'RF'
										ELSE CONVERT(NVARCHAR,T0.TransType) END+' '+CONVERT(NVARCHAR,T0.BaseRef)
			,'glosa'			=	T0.LineMemo--UPPER(REPLACE(REPLACE(T0.LineMemo,'ó','o'),'ú','u'))
			,'OCR1'				=	ISNULL(T0.ProfitCode,'')
			,'OCR2'				=	ISNULL(T0.OcrCode2,'')
			,'OCR3'				=	ISNULL(T0.OcrCode3,'')
			,'OCR4'				=	ISNULL(T0.OcrCode4,'')
			,'OCR5'				=	ISNULL(T0.OcrCode5,'')
			,'Proyecto'	        =   ISNULL(T1.Project,'')
		FROM 
			JDT1 T0 
			INNER JOIN OACT T1 ON T1.AcctCode = T0.Account AND LEFT(T1.FormatCode,1) = @DIGNATURALEZA AND T1.U_BPP_PCTC='N'
		WHERE 
			T0.RefDate>=@FECHAINI and T0.RefDate<=@FECHAFIN AND T0.U_BPP_DCPR='N'
			AND (((ISNULL(T0.Debit,0) - ISNULL(T0.Credit,0)) <> 0) OR ((ISNULL(T0.FCDebit,0) - ISNULL(T0.FCCredit,0)) <> 0) OR ((ISNULL(T0.SYSDeb,0) - ISNULL(T0.SYSCred,0)) <> 0))
			
			
	
	SELECT
	---ROW_NUMBER() OVER(ORDER BY astOrigen)
	--'linenum',astOrigen,Cuentas,
	--deb,cred,fdeb,fcred,fcurr,fechaCont,fechaDocu,fechaVenc,referencia,glosa,OCR1,OCR2,OCR3,OCR4,OCR5,Proyecto,
	CONVERT(NVARCHAR,1) +','+CONVERT(NVARCHAR,'')+','+CONVERT(NVARCHAR,Cuentas)+','+','+
	CONVERT(NVARCHAR,deb)+','+CONVERT(NVARCHAR,cred)+','+CONVERT(NVARCHAR,fdeb)+','+CONVERT(NVARCHAR,fcred)+','+
	CONVERT(NVARCHAR,fcurr)+','+CONVERT(NVARCHAR,fechaVenc)+','+CONVERT(NVARCHAR,fechaDocu)+','+
	CONVERT(NVARCHAR,Proyecto)+','+CONVERT(NVARCHAR,glosa)+','+CONVERT(NVARCHAR,astOrigen)+','+CONVERT(NVARCHAR,fechaCont)+','+
	CONVERT(NVARCHAR,OCR1)+','+CONVERT(NVARCHAR,OCR2)+','+CONVERT(NVARCHAR,OCR3)+','+CONVERT(NVARCHAR,OCR4)+','+CONVERT(NVARCHAR,OCR5) 'data'
		
	FROM @REST order by cuentas asc
	--WHERE ROUND(montoLocal,2)<>0 OR ROUND(montoExtranjero,2)<>0 OR ROUND(montoSistema,2)<>0
	--ORDER BY
	--CASE substring(Cuentas,1,1) WHEN '6' THEN Cuentas END DESC,
 --   CASE substring(Cuentas,1,1) WHEN '9' THEN Cuentas END ASC
END


--EXEC [dbo].[VS_FINANB1_DESTINOS_POR_PERIODO_DTW] '51'


