ALTER TABLE tbLanguage ALTER COLUMN ControlTextEN NVARCHAR(4000)
ALTER TABLE tbLanguage ALTER COLUMN ControlTextBG NVARCHAR(4000)
ALTER TABLE tbLanguage ALTER COLUMN ControlTextDE NVARCHAR(4000)

INSERT INTO tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('LoginPageText'
           ,'Organize your monthly budget. Plan your future spendings and set a budget for the various expenditures for the month.
<br />                       
<br />
<ul>
    <li>Have you ever asked yourself: &quot;Where did all my money go&quot;?</li>
    <li>Have you received a bill you have completely forgotten about?</li>
    <li>Ever wanted to save some money and never actually doing it because money has just
        vanished into thin air?</li>
    <li>And this credit card... what&#39;s going on with my balance and what did I pay last
        month?</li>
</ul>
It will be so much easier if you could plan your expenses based on the previous
months&#39; payments and thus set the right amount for the current month.<br />
<br />
... and if you don&#39;t have to remember all those payment deadlines.<br />
<br />
With MyHomeBills you register your monthly bills and the deadline for their payment;
plan the different expenditures so that you can set the right sum for them; compare
planned with actually paid sums; add details to each expenditure.<br />
<br />
Your expenses will be automatically categorised by keywords (e.g. &quot;phone&quot;,
&quot;electricity&quot;, &quot;internet&quot;, &quot;savings&quot;, etc.) Thus,
you get summarised information on how much you have spent for the previous months
on the respective item, highest and lowest sum paid, as well as average others have
spent on the same item.
<br />
<br />
It is so easy to do all that, because MyHomeBills brings it down to a simple spreadsheet
editing, as if it is just an office document.'
           ,'<strong>Организирайте месечния си бюджет. Планирайте предстоящите разходи и определете
бюджет за отделните сметки и харчове през месеца.</strong>
<br />
<br />
• Случвало ли Ви се е да се запитате: "Къде отидоха парите ми"?
<br />
• А някога да е пристигала сметка, за която сте забравили?
<br />
• Искало ли Ви се е да спестите пари за нещо, а никога да не успявате, защото те
се губят незнайно къде?
<br />
• И тази кредитна карта... защо все толкова много имам да плащам по нея?
<br />
<br />
Колко по-лесно би било да планирате разходите си на база предишни сметки и да определите
точната сума, която бихте отделили този месец.
<br />
... и да не Ви се налага да помните крайните срокове за плащане.
<br />
<br />
С MyHomeBills регистрирате месечните си сметки и срок за плащането им; планирате
перата в бюджета си, за да заделите съответна сума по тях; сравнявате планирани
с реално платени суми; добавяте разбивка към отделните сметки. Сметките ви автоматично
се категоризират по зададени ключови думи (пример: "телефон", "ток", "интернет",
"спестявания" и др.) Така получавате обобщена информация колко сте похарчили през
изминалия период за дадената сметка, най-ниска и най-висока сума по нея, както и
средна стойност на платено за същата сметка от останалите потребители. И е толкова
лесно да направите всичко това, защото MyHomeBills го свежда до попълването на обикновена
таблица, сякаш свободно попълвате офис документ.
<br />
<br />
<br />
<strong>Посетете нашите приятели за интересни и полезни съвети свързани с личните финанси:</strong>'
           ,'<strong> Organisieren Sie Ihre monatlichen Budgets. Planen Sie Ihre anstehenden Kosten und bestimmen Sie die
individuelle Haushaltsrechnung und Ausgaben im Laufe des Monats. </strong>
<br />
')



INSERT INTO tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('donate', 'You can support MyHomeBills by donating a small sum:', 'Ако желаете подкрепете MyHomeBills като дарите малка сума:', 'Hiermit können SIe MyHomeBills mit einer kleinen Summe unterstützen:')