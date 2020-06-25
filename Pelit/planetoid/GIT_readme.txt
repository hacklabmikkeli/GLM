git ei siis ole loppujenlopuksi vaikea k‰ytt‰‰.


================================================
//==\\//==\\//==\\//==\\//==\\//==\\//==\\//==\\
================================================

Git pushin kulku p‰hkin‰nkuoressa

git pull
git status (onko konflikteja, jos on ni sit vaikka kysyt‰‰n neuvoa)
[tehd‰‰n halutut muutokset]
git add . 
git status (varmuuden vuoksi)
git commit (kirjoitetaan viesti, esc ja ":wq")
git status (varmuuden vuoksi)
git push

================================================
//==\\//==\\//==\\//==\\//==\\//==\\//==\\//==\\
================================================

kaikki alkaa siit‰, ett‰ navigoidaan git bash johonkin sellaiseen kansioon, mihin haluaa asiat kopioida. Navigointi tapahtuu komennoilla:
cd ..				<- menn‰‰n kansiorakenteessa taaksep‰in
cd <halutun kansion nimi> 	<- menn‰‰n kansiorakenteessa haluttuun kansiooon
cd /<aseman nimi> 		<- p‰‰st‰‰n halutulle kovolle
 

Kun haluttu levy ja kansio on lˆytynyt, kirjoitetaan bashiin "git clone https://github.com/Atomilehma/Planetoid.git" 
T‰m‰ kloonaa tuon planetoidin koko kansiorakenteen siihen tiedostoon, miss‰ bash sill‰ hetkell‰ on menossa. 
GIT CLONE TARVITSEE TEHDƒ VAIN KERRAN/ SILLON KUN ON KUSSUT JOTAIN PAHASTI JA HALUAA KLOONATA PUHTAAN REPON.
(Jos siis tekee tyhmi‰ virheit‰ ja haluaa kloonata puhtaan branchin, niin poistaa kansiosta kaiken tiedoston ja tekee t‰m‰n git clonen uudestaan)

Kun clone on tehty kertaalleen, niin voidaan k‰ytt‰‰ komentoa
git clone		<- kloonaa git kansioon uudet tiedostot
git status		<- katsoo oman kansiorakenteen statuksen
git add . 		<- lis‰t‰‰n kaikki tehdyt muutokset commitoitavaksi. Vastaavasti voidaan k‰ytt‰‰ git add <tiedostonimi> jos halutaan lis‰t‰ vain joku muokattu tiedosto
git commit 		<- commit valmistaa valitut tiedostot l‰hetett‰v‰ksi, aukaisee luultavasti VIM:n mihin pit‰‰ kirjoittaa commit log ja sitten esc -> ":wq" jolloin tiedostot l‰htev‰t pushille
git push		<- tehdyt commitit pushataan repositoryyn. Vastaavasti voidaan k‰ytt‰‰ komentoa 
git push origin master	<- pushataan origin masteriin

Siis kun alkuh‰rdellit on kondiksessa yll‰olevat komennot ovat ainoat tarvittavat komennot. 



