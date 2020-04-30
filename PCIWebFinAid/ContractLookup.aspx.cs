using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class ContractLookup : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			SetErrorDetail(-88,0,"","");

			if ( ! Page.IsPostBack )
			{
				ShowControls(0,false);
				rdoView2.Checked = false;
				rdoView1.Checked = true;
				txtContractCode.Focus();
			}
		}

		private void ShowControls(byte view,bool show)
		{
			pnlView1.Visible = show;
			pnlView2.Visible = show;

			if ( view == 1 &&  show )
				pnlView2.Visible = ! show;
			else if ( view == 2 &&  show )
				pnlView1.Visible = ! show;

			btnMail.Visible  = ( show && view == 1 );
			btnPrint.Visible = show;
			if ( ! show )
				hdnProductCode.Value = "";
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

		protected void btnSend_Click(Object sender, EventArgs e)
		{
			lblError.Text = "Internal error, mail not sent";

			if ( ! pnlView1.Visible || hdnProductCode.Value.Length < 1 )
				return;

			txtTo.Text  = txtTo.Text.Trim();
			txtCC.Text  = txtCC.Text.Trim();
			txtBCC.Text = txtBCC.Text.Trim();

			if ( ! Tools.CheckEMail(txtTo.Text,2) || ! Tools.CheckEMail(txtCC.Text,3) || ! Tools.CheckEMail(txtBCC.Text,3) )
			{
				lblError2.Text = "Invalid email address(es)";
				lblJS.Text     = WebTools.JavaScriptSource("ShowElt('pnlMail',true)");
				return;
			}

			try
			{
				string mailText = File.ReadAllText(Tools.SystemFolder("Templates")+"ConfirmationMail.htm");

				foreach (Control ctlOuter in pnlView1.Controls)
				{
					if ( ctlOuter.GetType() == typeof(Literal) && mailText.Contains("#"+ctlOuter.ID+"#") )
						mailText = mailText.Replace("#"+ctlOuter.ID+"#",((Literal)ctlOuter).Text);
					else if ( ctlOuter.GetType() == typeof(Label) && mailText.Contains("#"+ctlOuter.ID+"#") )
						mailText = mailText.Replace("#"+ctlOuter.ID+"#",((Label)ctlOuter).Text);
					else
						foreach (Control ctlInner in ctlOuter.Controls)
							if ( ctlInner.GetType() == typeof(Literal) && mailText.Contains("#"+ctlInner.ID+"#") )
								mailText = mailText.Replace("#"+ctlInner.ID+"#",((Literal)ctlInner).Text);
							else if ( ctlInner.GetType() == typeof(Label) && mailText.Contains("#"+ctlInner.ID+"#") )
								mailText = mailText.Replace("#"+ctlInner.ID+"#",((Label)ctlInner).Text);
				}

				using ( Mail msg = new Mail() )
				{
					msg.Heading = "Contract " + txtContractCode.Text;
					msg.Body    = mailText;
					msg.To      = txtTo.Text;
					msg.From    = hdnMailFrom.Value;
					msg.CC      = txtCC.Text;
					msg.BCC     = txtBCC.Text;
					int ret     = msg.Send();
					if ( ret == 0 )
						lblError.Text = "Mail successfully sent to " + txtTo.Text;
					else
						lblError.Text = "Mail failed";
				}
			}
			catch (Exception ex)
			{
				lblError.Text = "Mail failed (" + ex.ToString() + ")";
			}
			lblError.Visible = true;
		}

		protected void btnSearch_Click(Object sender, EventArgs e)
		{
			ShowControls(0,false);

			if ( ValidateData() > 0 )
				return;

			Literal ctlLiteral;
			string  fieldCode;
			string  fieldValue;
			string  productCode          = "";
			string  productOptionCode    = "";
			string  titleCode            = "";
			string  titleDesc            = "";
			string  initials             = "";
			string  firstName            = "";
			string  surname              = "";
			string  payDateCode          = "";
			string  paymentMethodCode    = "";
			string  employmentStatusCode = "";
			string  ccNumber             = "";
			string  ccAssociation        = "";
			string  sql                  = "exec WP_Get_ContractApplication @ContractCode = " + Tools.DBString(txtContractCode.Text);
	
			using ( MiscList miscList = new MiscList() )
				if ( miscList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail(30010,30010,"Internal database error (WP_Get_ContractApplication)",sql);
				else if ( miscList.EOF )
					SetErrorDetail(30011,30011,"Contract not found. Please try again",sql);
				else
				{
					lblWebsiteCode.Text                       = miscList.GetColumn("WebsiteCode");
					lblContractCode.Text                      = miscList.GetColumn("ContractCode");
					lblp6Ref.Text                             = miscList.GetColumn("ContractCode");
					lblContractApplicationStatusCode.Text     = miscList.GetColumn("ContractApplicationStatusCode");
					lblProspectingStatusCode.Text             = miscList.GetColumn("ProspectingStatusCode");
					lblClientCode.Text                        = miscList.GetColumn("ClientCode");
					lblp6ID.Text                              = miscList.GetColumn("ClientCode");
					lblClientCodeTypeCode.Text                = miscList.GetColumn("ClientCodeTypeCode");
					productCode                               = miscList.GetColumn("ProductCode");
					hdnProductCode.Value                      = productCode;
					lblProductCode.Text                       = productCode;
					productOptionCode                         = miscList.GetColumn("ProductOptionCode");
					lblProductOptionCode.Text                 = productOptionCode;
					lblProductOptionMandateTypeCode.Text      = miscList.GetColumn("ProductOptionMandateTypeCode");
					surname                                   = miscList.GetColumn("Surname");
					lblSurname.Text                           = surname;
					lblp6Surname.Text                         = surname;
					firstName                                 = miscList.GetColumn("FirstName");
					lblFirstName.Text                         = firstName;
					lblp6FirstName.Text                       = firstName;
					lblMiddleNames.Text                       = miscList.GetColumn("MiddleNames");
					initials                                  = miscList.GetColumn("Initials").ToUpper();
					lblInitials.Text                          = initials;
					titleCode                                 = miscList.GetColumn("TitleCode");
					lblTitleCode.Text                         = titleCode;
					lblDateOfBirth.Text                       = miscList.GetColumn("DateOfBirth");
					lblHomeLanguageCode.Text                  = miscList.GetColumn("HomeLanguageCode");
					lblHomeLanguageDialectCode.Text           = miscList.GetColumn("HomeLanguageDialectCode");
					lblCorrespondenceLanguageCode.Text        = miscList.GetColumn("CorrespondenceLanguageCode");
					lblCorrespondenceLanguageDialectCode.Text = miscList.GetColumn("CorrespondenceLanguageDialectCode");
					lblGenderCode.Text                        = miscList.GetColumn("GenderCode");
					lblTelephoneNumberM.Text                  = miscList.GetColumn("TelephoneNumberM");
					lblp6CellNo.Text                          = miscList.GetColumn("TelephoneNumberM");
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
					lblp6EMail.Text                           = miscList.GetColumn("EMailAddress");
					txtTo.Text                                = miscList.GetColumn("EMailAddress");
					payDateCode                               = miscList.GetColumn("PayDateCode");
					lblPayDateCode.Text                       = payDateCode;
					paymentMethodCode                         = miscList.GetColumn("PaymentMethodCode");
					lblPaymentMethodCode.Text                 = paymentMethodCode;
					lblPaymentCycleCode.Text                  = miscList.GetColumn("PaymentCycleCode");
					lblBankCode.Text                          = miscList.GetColumn("BankCode");
					lblBankBranchCode.Text                    = miscList.GetColumn("BankBranchCode");
					lblAccountHolder.Text                     = miscList.GetColumn("AccountHolder");
					lblp6CCName.Text                          = miscList.GetColumn("AccountHolder");
					lblBankAccountHolderRelationShipCode.Text = miscList.GetColumn("BankAccountHolderRelationShipCode");
					lblBankAccountNumber.Text                 = miscList.GetColumn("BankAccountNumber");
					lblBankAccountTypeCode.Text               = miscList.GetColumn("BankAccountTypeCode");
					lblCardAssociationCode.Text               = miscList.GetColumn("CardAssociationCode");
					lblCardTypeCode.Text                      = miscList.GetColumn("CardTypeCode");
					lblp6CCType.Text                          = miscList.GetColumn("CardTypeCode");
					ccNumber                                  = miscList.GetColumn("CardNumber");
					lblCardExpiryMonth.Text                   = miscList.GetColumn("CardExpiryMonth");
					lblCardExpiryYear.Text                    = miscList.GetColumn("CardExpiryYear");
					lblp6CCExpiry.Text                        = lblCardExpiryYear.Text + "/" + lblCardExpiryMonth.Text;
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
					lblp6Income.Text                          = miscList.GetColumn("DisposableIncome");
					lblLegalRestrictionCode.Text              = miscList.GetColumn("LegalRestrictionCode");
					employmentStatusCode                      = miscList.GetColumn("ClientEmploymentStatusCode");
					lblClientEmploymentStatusCode.Text        = employmentStatusCode;
					lblTsCsRead.Text                          = miscList.GetColumn("TsCsRead");
					lblRefundPolicyRead.Text                  = miscList.GetColumn("RefundPolicyRead");
					lblCancellationPolicyRead.Text            = miscList.GetColumn("CancellationPolicyRead");
					lblClientIPAddress.Text                   = miscList.GetColumn("ClientIPAddress");
					lblp6IP.Text                              = miscList.GetColumn("ClientIPAddress");
					lblClientDevice.Text                      = miscList.GetColumn("ClientDevice");
					lblContactCentreCode.Text                 = miscList.GetColumn("ContactCentreCode");
					lblContractApplicationDate.Text           = miscList.GetColumn("ContractApplicationDate");
					lblp6Date.Text                            = miscList.GetColumn("ContractApplicationDate");
					lblSalesAgentCode.Text                    = miscList.GetColumn("SalesAgentCode");
					lblCapturingAgentCode.Text                = miscList.GetColumn("CapturingAgentCode");
					lblMarketingMixCode.Text                  = miscList.GetColumn("MarketingMixCode");
					lblContractProcurementChannelCode.Text    = miscList.GetColumn("ContractProcurementChannelCode");
					lblContractPin.Text                       = miscList.GetColumn("ContractPin");
					lblp6Pin.Text                             = miscList.GetColumn("ContractPin");
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
				}

			if ( lblError.Text.Length < 1 && productCode.Length < 1 )
				SetErrorDetail(30014,30014,"Contract corrupted - product code is blank/empty",sql);

			if ( lblError.Text.Length > 0 )
				return;

			if ( ccNumber.Length >= 6 )
				ccAssociation = ccNumber.Substring(0,6);

//	Mask card number
			if ( ccNumber.Length > 12 )
				ccNumber = ccNumber.Substring(0,6) + "******" + ccNumber.Substring(12);
			else if ( ccNumber.Length > 8 )
				ccNumber = ccNumber.Substring(0,4) + "******";
			else if ( ccNumber.Length > 4 )
				ccNumber = ccNumber.Substring(0,2) + "******";
			else if ( ccNumber.Length > 2 )
				ccNumber = "******";

			lblCardNumber.Text = ccNumber;
			lblp6CCNumber.Text = ccNumber;

			if ( rdoView2.Checked )
				ShowControls(2,true);

			else if ( rdoView1.Checked )
			{
				using ( MiscList miscList = new MiscList() )
				{
				//	Field labels
					sql = "exec sp_WP_CRM_Get_ProductWebsiteRegContent @ProductCode=" + Tools.DBString(productCode);
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30020,30020,"Internal database error (sp_WP_CRM_Get_ProductWebsiteRegContent)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30021,30021,"Product details not found (" + productCode + "). Please try again",sql);
					else
						while ( ! miscList.EOF )
						{
							fieldCode    = miscList.GetColumn("WebsiteFieldCode");
							fieldValue   = miscList.GetColumn("WebsiteFieldValue");
							ctlLiteral   = (Literal)FindControl("lbl"+fieldCode);
							if ( ctlLiteral != null )
								ctlLiteral.Text = fieldValue;
							miscList.NextRow();
						}

				//	Title
					sql = "exec sp_WP_CRM_Get_Title";
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30030,30030,"Internal database error (sp_WP_CRM_Get_Title)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30031,30031,"Title descriptions not found. Please try again",sql);
					else
						while ( ! miscList.EOF )
							if ( miscList.GetColumn("TitleCode") == titleCode )
							{
								titleDesc = miscList.GetColumn("TitleDescription");
								break;
							}
							else
								miscList.NextRow();

				//	Payment method
					sql = "exec sp_WP_CRM_Get_PaymentMethod @ProductCode=" + Tools.DBString(productCode);
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30040,30040,"Internal database error (sp_WP_CRM_Get_PaymentMethod)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30041,30041,"Payment methods not found. Please try again",sql);
					else
						while ( ! miscList.EOF )
							if ( miscList.GetColumn("PaymentMethodCode") == paymentMethodCode )
							{
								lblp6Payment.Text = miscList.GetColumn("PaymentMethodDescription");
								break;
							}
							else
								miscList.NextRow();

				//	Pay Date
					sql = "exec sp_WP_CRM_Get_PayDate";
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30050,30050,"Internal database error (sp_WP_CRM_Get_PayDate)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30051,30051,"Pay dates not found. Please try again",sql);
					else
						while ( ! miscList.EOF )
							if ( miscList.GetColumn("PayDateCode") == payDateCode )
							{
								lblp6PayDay.Text = miscList.GetColumn("PayDateDescription");
								break;
							}
							else
								miscList.NextRow();

				//	Employment status
					sql = "exec sp_WP_CRM_Get_EmploymentStatus";
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30060,30060,"Internal database error (sp_WP_CRM_Get_EmploymentStatus)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30061,30061,"Employment statuses not found. Please try again",sql);
					else
						while ( ! miscList.EOF )
							if ( miscList.GetColumn("EmploymentStatusCode") == employmentStatusCode )
							{
								lblp6Status.Text = miscList.GetColumn("EmploymentStatusDescription");
								break;
							}
							else
								miscList.NextRow();

				//	Card association
					sql = "exec WP_Get_CardAssociation @BIN=" + Tools.DBString(ccAssociation);
					if ( ccAssociation.Length < 1 )
						lblp6CCType.Text = "N/A";
					else if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30070,30070,"Internal database error (WP_Get_CardAssociation)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30071,30071,"Bank card association not found (" + ccAssociation + "). Please try again",sql);
					else
						lblp6CCType.Text = miscList.GetColumn("Brand");

				//	Product option
					sql = "exec sp_WP_CRM_Get_WebsiteProductoptionA"
					    + " @ProductCode="       + Tools.DBString(productCode)
					    + ",@ProductOptionCode=" + Tools.DBString(productOptionCode);
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30080,30080,"Internal database error (sp_WP_CRM_Get_WebsiteProductoptionA)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30081,30081,"Product option details not found. Please try again",sql);
					else
					{
						lbl100325.Text = miscList.GetColumn("FieldValue");
						if ( lbl100325.Text.Length > 0 )
							lbl100325.Text = lbl100325.Text.Replace(Environment.NewLine,"<br />");
						else
							SetErrorDetail(30082,30082,"Product option data is empty/blank (sp_WP_CRM_Get_WebsiteProductOptionA, column 'FieldValue')",sql);
					}

				//	EMail details
					sql = "exec sp_WP_Get_ProductEmail @LanguageCode='ENG', @ProductCode=" + Tools.DBString(productCode);
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30120,30120,"Internal database error (sp_WP_Get_ProductEmail)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30121,30121,"Product email details not found. Please try again",sql);
					else
					{
						txtFrom.Text       = miscList.GetColumn("SenderEmailAddress");
						hdnMailFrom.Value  = miscList.GetColumn("SenderEmailAddress");
						hdnMailReply.Value = miscList.GetColumn("ReplyEMailAddress");
					}

				//	Product policy
					sql = "exec sp_WP_CRM_Get_ProductPolicy @ProductCode=" + Tools.DBString(productCode);
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30090,30090,"Internal database error (sp_WP_CRM_Get_ProductPolicy)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30091,30091,"Product policy details not found. Please try again",sql);
					else
					{
						string refundPolicy          = miscList.GetColumn("RefundPolicyText");
						string moneyBackPolicy       = miscList.GetColumn("MoneyBackPolicyText");
						string cancellationPolicy    = miscList.GetColumn("CancellationPolicyText");
						lblp6RefundPolicy.Text       = refundPolicy.Replace(Environment.NewLine,"<br />") + "<br />&nbsp;";
						lblp6MoneyBackPolicy.Text    = moneyBackPolicy.Replace(Environment.NewLine,"<br />") + "<br />&nbsp;";
						lblp6CancellationPolicy.Text = cancellationPolicy.Replace(Environment.NewLine,"<br />");
					}

				//	Collection mandate
					sql = "exec sp_WP_CRM_Get_ProductOptionMandateA @ProductCode=" + Tools.DBString(productCode);
					if ( miscList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail(30100,30100,"Internal database error (sp_WP_CRM_Get_ProductOptionMandateA)",sql);
					else if ( miscList.EOF )
						SetErrorDetail(30101,30101,"Collection mandate details not found. Please try again",sql);
					else
						while ( ! miscList.EOF )
						{
							if ( ( miscList.GetColumn("ProductOptionCode").ToUpper() == productOptionCode.ToUpper()   &&
							       miscList.GetColumn("PaymentMethodCode").ToUpper() == paymentMethodCode.ToUpper() ) ||
							       miscList.GetColumn("PaymentMethodCode") == "*" )
							{
								string mHead   = "";
								string mandate = miscList.GetColumn("CollectionMandateText",0);
								int    k       = mandate.IndexOf("\n"); // Do NOT use Environment.NewLine here!
								if ( k > 0 && mandate.Length > k+1 )
								{
									mHead   = mandate.Substring(0,k);
									mandate = mandate.Substring(k+1).Replace(Environment.NewLine,"<br />");
								}
								lblp6MandateHead.Text = mHead;
								lblp6Mandate.Text     = mandate;
								break;
							}
							miscList.NextRow();
						}
						if ( lblp6Mandate.Text.Length < 1 )
							SetErrorDetail(30110,30111,"Unable to retrieve collection mandate",sql+" (looking for ProductOption="+productOptionCode+" and PaymentMethod="+paymentMethodCode+"). SQL failed or returned no data or<br />the CollectionMandateText column was missing/empty/NULL");
				}

				lblp6Title.Text = titleDesc;
				lbl100209.Text  = lbl100209.Text.Replace("[Title]",titleDesc).Replace("[Surname]",surname);
				if ( initials.Length > 0 )
					lbl100209.Text = lbl100209.Text.Replace("[Initials]",initials);
				else if ( firstName.Length > 0 )
					lbl100209.Text = lbl100209.Text.Replace("[Initials]",firstName.Substring(0,1).ToUpper());
				else
					lbl100209.Text = lbl100209.Text.Replace("[Initials]","");
				ShowControls(1,true);
			}
		}

		private void SetErrorDetail(int errCode,int logNo,string errBrief,string errDetail,byte briefMode=2,byte detailMode=2)
		{
			if ( errCode == 0 )
				return;

			if ( errCode <  0 )
			{
				lblJS.Text       = "";
				lblError.Text    = "";
				lblError2.Text   = "";
				lblErrorDtl.Text = "";
				lblError.Visible = false;
				btnError.Visible = false;
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