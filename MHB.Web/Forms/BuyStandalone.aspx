<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeBehind="BuyStandalone.aspx.vb" Inherits="MHB.Web.Forms.BuyStandalone" EnableViewStateMac="false" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="LoginPageMainTable">
        <div style="padding: 40px;">
            <p><span style="font-size: 14pt;">&nbsp;Здравейте,</span></p>
            <p>&nbsp;</p>
            <p>
                <br />
                <br />
                <span style="font-size: 14pt;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>MyHomeBills може да бъде инсталиран и да работи самостоятелно на вашия компютър или сървър.&nbsp;</strong>Може да го достъпвате по абсолютно същия начин във вашия браузър и от всички компютри във вашата мрежа.</span>
            </p>
            <p style="text-align: center;"><span style="font-size: 18pt; color: #007bff;"><strong>Поддържани операционни системи:</strong> </span></p>
            <p style="text-align: center;"><span style="font-size: 18pt; color: #007bff;">Само 64 битови версии на: Windows 7, 8, 8.1, 10</span></p>
            <p style="text-align: center;">
                <asp:ImageButton ID="ImageButtonDownload" OnClick="ImageButtonDownload_OnClick" runat="server" ImageUrl="../Images/btn_download_standalone.png" AlternateText="Download MyHomeBills" Width="221" Height="69" />
            </p>

            <p>
                <br />
                <span style="font-size: 14pt;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; За целта <strong>не е необходима връзка с интернет</strong> или със сървъра на MyHomeBills. Всички ваши данни ще бъдат <strong>съхранявани локално на вашия компютър</strong> и управлението и архивирането им е във ваши ръце. За момента не е възможна синхронизация на данните в реално време със сървъра на MyHomeBills, но <strong>може да прехвърлите еднократно всички вече съществуващи ваши данни</strong> до момента.<br />
                </span>
            </p>
            <p><span style="font-size: 14pt;">За да инсталирате и използвате MyHomeBills локално на вашия компютър е необходимо да изпълните следните няколко стъпки:</span></p>
            <p>&nbsp;</p>
            <p style="text-align: center;"><span style="color: #000080; font-size: 18pt;"><strong>Да изискате безплатен или платен лиценз за избран от вас срок на ползване</strong></span></p>
            <p>
                <span style="font-size: 14pt;">MyHomeBills е и винаги ще остане напълно безплатен, както през изминалите 8 години от създаването си.
                <br />
                    <br />
                    <strong style="color: darkblue">След изтичането на безплатния тримесечен период може да поръчате следващи неограничен брой 3 месечни лицензи.
                    Единственото неудобство произтича от времето за получаване на новия 3 месечен лиценз - до 12 часа и липсата на поддръжка и обновяване на инсталацията.</strong><br />
                    <br />
                    Тъй като продължаващата му разработка и
                поддръжката на сървърите изискват ежемесечни финансови постъпления, решиме да предложим и възможността за платена локална инсталация с пълна поддръжка и актуализация, като начин да покрием някаква част от разходите.<br />
                </span>
            </p>
            <table style="height: 56px; background-color: #baebff;" width="100%" cellspacing="0" cellpadding="20">
                <tbody>
                    <tr style="height: 31px;">
                        <td style="width: 230.233px; text-align: center; height: 31px; background-color: palegreen;"><strong><span style="font-size: 18pt; color: #003366;">3 месеца</span></strong></td>
                        <td style="width: 230.233px; text-align: center; height: 31px;"><strong><span style="font-size: 18pt; color: #003366;">1 година</span></strong></td>
                        <td style="width: 230.233px; text-align: center; height: 31px;"><strong><span style="font-size: 18pt; color: #003366;">3 години</span></strong></td>
                        <td style="width: 351.533px; text-align: center; height: 31px;"><strong><span style="font-size: 18pt; color: #003366;">Вечен лиценз</span></strong></td>
                    </tr>
                    <tr style="background-color: #00aaff; height: 40px;">
                        <td style="width: 230.233px; text-align: center; height: 40px; background-color: palegreen"><span style="color: forestgreen;"><strong><span style="font-size: 24pt;">Безплатно</span></strong></span></td>
                        <td style="width: 230.233px; text-align: center; height: 40px;"><span style="color: #ffffff;"><strong><span style="font-size: 24pt;">13лв</span></strong></span></td>
                        <td style="width: 230.233px; text-align: center; height: 40px;"><span style="color: #ffffff;"><strong><span style="font-size: 24pt;">30лв</span></strong></span></td>
                        <td style="width: 351.533px; text-align: center; height: 40px;"><span style="color: #ffff99;"><strong><span style="font-size: 24pt;">90лв</span></strong></span></td>
                    </tr>
                </tbody>
            </table>
            <p style="text-align: center;"><span style="font-size: 14pt;">Моля свържете се с нас на адрес на електронна поща: <a href="mailto:support@myhomebills.info">support@myhomebills.info</a> за повече информация относно методите на плащане.</span></p>
            <p style="text-align: center;">
                <br />
                <br />
                <br />
                <span style="font-size: 14pt;"><span style="color: #000080;"><strong><span style="font-size: 18pt;">Да генерирате уникален индентификатор на вашия компютър, който се изисква за лицензирането</span><br />
                </strong></span></span>
            </p>
            <p>&nbsp;<span style="font-size: 14pt;">Лицензът важи за определен компютър, като по този начин е невъзможно MyHomeBills да бъде инсталиран на друг. Целта е както и да се елеминира свободното копиране и разпространение на софтуера, така и вашите данни да бъдат по-защитени.</span></p>
            <p><span style="font-size: 14pt;"><strong>За целта единствено е нужно да запишете и разархивирате</strong> <a href="https://myhomebills.info/MachineFingerprintGenerator.zip">MachineFingerprintGenerator.zip</a> , да стартирате <span style="color: #007bff;">MachineFingerprintGenerator.exe</span> и да <strong>изпратите генерирания файл с име:</strong> <em><span style="color: #007bff;">MachineKey</span> </em>на e-mail адрес: support@myhomebills.info</span></p>
            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <p>&nbsp;</p>
            <p>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
            </p>
        </div>
    </div>
</asp:Content>