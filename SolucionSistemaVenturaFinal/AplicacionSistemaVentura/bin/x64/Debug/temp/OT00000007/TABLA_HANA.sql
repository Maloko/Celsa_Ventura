CREATE COLUMN  TABLE "SBO_SEGORLL". "CUENTADESTINO"
(RecordKey int,line_id int, AccountCode nvarchar(30), NO_USAR_AccountCode nvarchar(420),
AdditionalReference nvarchar(100), BaseSum nvarchar(10), ContraAccount nvarchar(10), CostingCode nvarchar(16),
Debit NUMERIC(19,6), Credit NUMERIC(19,6), DueDate DATETIME, FCDebit NUMERIC(19,6), FCCredit NUMERIC(19,6),
DebitSYS NUMERIC(19,6), CreditSYS NUMERIC(19,6), FCCurrency nvarchar(6), GrossValue nvarchar(10),
LineMemo nvarchar(100), ProjectCode nvarchar(40), Reference1 nvarchar(200), Reference2 nvarchar(200),
ReferenceDate1 DATETIME, ReferenceDate2 nvarchar(10), ShortName	nvarchar(10), SystemBaseAmount NUMERIC(19,6),
SystemVatAmount nvarchar(10), TaxDate DATETIME, TaxGroup nvarchar(10), VatAmount nvarchar(10),
VatDate nvarchar(10), VatLine nvarchar(10), no_usar_transid	int, extorno int,OLD_line_id int,
TransType nvarchar(40), BaseRef nvarchar(22), DestinoCreado nvarchar(22), MPE INT, BORRADO INT, 
CtaImputable nvarchar(30));