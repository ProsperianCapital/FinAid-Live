using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class ContractLookup : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			SetErrorDetail(-88,0,"","");

			if ( ! Page.IsPostBack )
				pnlData.Visible = false;
		}

		private int ValidateData()
		{
			txtContractCode.Text = txtContractCode.Text.Trim();
			string err           = "";
			if ( txtContractCode.Text.Length < 2 )
				err = "Invalid contract code<br />";
			txtContractCode.Focus();
			SetErrorDetail(err.Length,100,err,err);
			return err.Length;
		}


		protected void btnSearch_Click(Object sender, EventArgs e)
		{
			pnlData.Visible = false;

			if ( ValidateData() > 0 )
				return;

			string sql = "exec WP_Get_ContractApplication @ContractCode = " + Tools.DBString(txtContractCode.Text);
	
			using ( MiscList miscList = new MiscList() )
				if ( miscList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail(30060,30060,"Internal database error (WP_Get_ContractApplication)",sql,2,2);
				else if ( miscList.EOF )
					SetErrorDetail(30061,30061,"Contract not found. Please try again","",2,0);
				else
				{
					lblWebsiteCode.Text                   = miscList.GetColumn("WebsiteCode");
					lblContractCode.Text                  = miscList.GetColumn("ContractCode");
					lblContractApplicationStatusCode.Text = miscList.GetColumn("ContractApplicationStatusCode");
					lblProspectingStatusCode.Text         = miscList.GetColumn("ProspectingStatusCode");
					lblClientCode.Text                    = miscList.GetColumn("ClientCode");
					lblClientCodeTypeCode.Text            = miscList.GetColumn("ClientCodeTypeCode");
					lblProductCode.Text                   = miscList.GetColumn("ProductCode");
					lblProductOptionCode.Text             = miscList.GetColumn("ProductOptionCode");
					lblProductOptionMandateTypeCode.Text  = miscList.GetColumn("ProductOptionMandateTypeCode");
					lblSurname.Text                       = miscList.GetColumn("Surname");
					lblFirstName.Text                     = miscList.GetColumn("FirstName");
					lblMiddleNames.Text                   = miscList.GetColumn("MiddleNames");
					lblInitials.Text                      = miscList.GetColumn("Initials");
					lblTitleCode.Text                     = miscList.GetColumn("TitleCode");
					lblDateOfBirth.Text                       = miscList.GetColumn("DateOfBirth");
					lblHomeLanguageCode.Text                  = miscList.GetColumn("HomeLanguageCode");
					lblHomeLanguageDialectCode.Text           = miscList.GetColumn("HomeLanguageDialectCode");
					lblCorrespondenceLanguageCode.Text        = miscList.GetColumn("CorrespondenceLanguageCode");
					lblCorrespondenceLanguageDialectCode.Text = miscList.GetColumn("CorrespondenceLanguageDialectCode");
					lblGenderCode.Text                        = miscList.GetColumn("GenderCode");
					lblTelephoneNumberM.Text                  = miscList.GetColumn("TelephoneNumberM");
					lblTelephoneNumberH.Text                  = miscList.GetColumn("TelephoneNumberH");
					lblTelephoneNumberW.Text                  = miscList.GetColumn("TelephoneNumberW");
					lblAddressLine1P.Text                     = miscList.GetColumn("AddressLine1P");
					lblAddressLine2P.Text                     = miscList.GetColumn("AddressLine2P");
					lblAddressLine3P.Text                     = miscList.GetColumn("AddressLine3P");
					lblAddressLine4P.Text                     = miscList.GetColumn("AddressLine4P");
					lblAddressLine5P.Text                     = miscList.GetColumn("AddressLine5P");
					lblAddressLine1W.Text                     = miscList.GetColumn("AddressLine1W");
					lblAddressLine2W.Text                     = miscList.GetColumn("AddressLine2W");
					lblAddressLine3W.Text                     = miscList.GetColumn("AddressLine3W");
					lblAddressLine4W.Text                     = miscList.GetColumn("AddressLine4W");
					lblAddressLine5W.Text                     = miscList.GetColumn("AddressLine5W");
					lblCountryCode.Text                       = miscList.GetColumn("CountryCode");
					lblEMailAddress.Text                      = miscList.GetColumn("EMailAddress");
					lblPayDateCode.Text                       = miscList.GetColumn("PayDateCode");
					lblPaymentMethodCode.Text                 = miscList.GetColumn("PaymentMethodCode");
					lblPaymentCycleCode.Text                  = miscList.GetColumn("PaymentCycleCode");
					lblBankCode.Text                          = miscList.GetColumn("BankCode");
					lblBankBranchCode.Text                    = miscList.GetColumn("BankBranchCode");
					lblAccountHolder.Text                     = miscList.GetColumn("AccountHolder");
					lblBankAccountHolderRelationShipCode.Text = miscList.GetColumn("BankAccountHolderRelationShipCode");
					lblBankAccountNumber.Text                 = miscList.GetColumn("BankAccountNumber");
					lblBankAccountTypeCode.Text               = miscList.GetColumn("BankAccountTypeCode");
					lblCardAssociationCode.Text               = miscList.GetColumn("CardAssociationCode");
					lblCardTypeCode.Text                      = miscList.GetColumn("CardTypeCode");
					lblCardNumber.Text                        = miscList.GetColumn("CardNumber");
					lblCardExpiryMonth.Text                   = miscList.GetColumn("CardExpiryMonth");
					lblCardExpiryYear.Text                    = miscList.GetColumn("CardExpiryYear");
					lblCardCVVCode.Text                       = miscList.GetColumn("CardCVVCode");
					lblThirdPartyCollectorCode.Text           = miscList.GetColumn("ThirdPartyCollectorCode");
					lblThirdPartyCollectorReference.Text      = miscList.GetColumn("ThirdPartyCollectorReference");
					lblGrossIncome.Text                       = miscList.GetColumn("GrossIncome");
					lblNetIncome.Text                         = miscList.GetColumn("NetIncome");
					lblExpenditureCellPhone.Text              = miscList.GetColumn("ExpenditureCellPhone");
					lblExpenditureGroceries.Text              = miscList.GetColumn("ExpenditureGroceries");
					lblExpenditureHousing.Text                = miscList.GetColumn("ExpenditureHousing");
					lblExpenditureInsurance.Text              = miscList.GetColumn("ExpenditureInsurance");
					lblExpenditureOther.Text                  = miscList.GetColumn("ExpenditureOther");
					lblDisposableIncome.Text                  = miscList.GetColumn("DisposableIncome");
					lblLegalRestrictionCode.Text              = miscList.GetColumn("LegalRestrictionCode");
					lblClientEmploymentStatusCode.Text        = miscList.GetColumn("ClientEmploymentStatusCode");
					lblTsCsRead.Text                          = miscList.GetColumn("TsCsRead");
					lblRefundPolicyRead.Text                  = miscList.GetColumn("RefundPolicyRead");
					lblCancellationPolicyRead.Text            = miscList.GetColumn("CancellationPolicyRead");
					lblClientIPAddress.Text                   = miscList.GetColumn("ClientIPAddress");
					lblClientDevice.Text                      = miscList.GetColumn("ClientDevice");
					lblContactCentreCode.Text                 = miscList.GetColumn("ContactCentreCode");
					lblContractApplicationDate.Text           = miscList.GetColumn("ContractApplicationDate");
					lblSalesAgentCode.Text                    = miscList.GetColumn("SalesAgentCode");
					lblCapturingAgentCode.Text                = miscList.GetColumn("CapturingAgentCode");
					lblMarketingMixCode.Text                  = miscList.GetColumn("MarketingMixCode");
					lblContractProcurementChannelCode.Text    = miscList.GetColumn("ContractProcurementChannelCode");
					lblContractPin.Text                       = miscList.GetColumn("ContractPin");
					lblWebsiteHostName.Text                   = miscList.GetColumn("WebsiteHostName");
					lblWebsiteVisitorCode.Text                = miscList.GetColumn("WebsiteVisitorCode");
					lblWebsiteVisitorSessionCode.Text         = miscList.GetColumn("WebsiteVisitorSessionCode");
					lblGoogleUtmSource.Text                   = miscList.GetColumn("GoogleUtmSource");
					lblGoogleUtmMedium.Text                   = miscList.GetColumn("GoogleUtmMedium");
					lblGoogleUtmCampaign.Text                 = miscList.GetColumn("GoogleUtmCampaign");
					lblGoogleUtmTerm.Text                     = miscList.GetColumn("GoogleUtmTerm");
					lblGoogleUtmContent.Text                  = miscList.GetColumn("GoogleUtmContent");
					lblAdvertCode.Text                        = miscList.GetColumn("AdvertCode");
					lblEventDate.Text                         = miscList.GetColumn("EventDate");
					lblEventTrigger.Text                      = miscList.GetColumn("EventTrigger");
					lblEventUserCode.Text                     = miscList.GetColumn("EventUserCode");
					pnlData.Visible                           = true;
				}
		}

		private void SetErrorDetail(int errCode,int logNo,string errBrief,string errDetail,byte briefMode=2,byte detailMode=1)
		{
			if ( errCode == 0 )
				return;

			if ( errCode <  0 )
			{
				lblError.Text        = "";
				lblErrorDtl.Text     = "";
				lblError.Visible     = false;
				btnError.Visible     = false;
				return;
			}

			Tools.LogInfo("ContractLookup.SetErrorDetail","(errCode="+errCode.ToString()+", logNo="+logNo.ToString()+") "+errDetail,244);

			if ( briefMode == 2 ) // Append
				lblError.Text = lblError.Text + ( lblError.Text.Length > 0 ? "<br />" : "" ) + errBrief;
			else
				lblError.Text = errBrief;
			lblError.Visible = ( lblError.Text.Length > 0 );

			if ( detailMode > 0 )
			{
				errDetail = errDetail.Replace(",","<br />,").Replace(";","<br />;").Trim();
				if ( detailMode == 2 ) // Append
					errDetail = lblErrorDtl.Text + ( lblErrorDtl.Text.Length > 0 ? "<br /><br />" : "" ) + errDetail;
				lblErrorDtl.Text = errDetail;
				if ( ! lblErrorDtl.Text.StartsWith("<div") )
					lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white;height:20px'>Error Details<img src='Images/Close1.png' title='Close' style='float:right' onclick=\"JavaScript:ShowElt('lblErrorDtl',false)\" /></div>" + lblErrorDtl.Text;
			}
			btnError.Visible = ( lblErrorDtl.Text.Length > 0 ) && lblError.Visible && ! Tools.SystemIsLive();
		}
	}
}