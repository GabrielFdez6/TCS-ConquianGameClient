ø
xC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\Properties\AssemblyInfo.cs
[

 
assembly

 	
:

	 

AssemblyTitle

 
(

 
$str

 *
)

* +
]

+ ,
[ 
assembly 	
:	 

AssemblyDescription 
( 
$str !
)! "
]" #
[ 
assembly 	
:	 
!
AssemblyConfiguration  
(  !
$str! #
)# $
]$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str ,
), -
]- .
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 2
)2 3
]3 4
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
["" 
assembly"" 	
:""	 

	ThemeInfo"" 
("" &
ResourceDictionaryLocation## 
.## 
None## #
,### $&
ResourceDictionaryLocation&& 
.&& 
SourceAssembly&& -
))) 
])) 
[33 
assembly33 	
:33	 

AssemblyVersion33 
(33 
$str33 $
)33$ %
]33% &
[44 
assembly44 	
:44	 

AssemblyFileVersion44 
(44 
$str44 (
)44( )
]44) *ë
C:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Authentication\LogIn.xaml.cs
	namespace 	
Conqui√°nCliente
 
{ 
public 

partial 
class 
LogIn 
:  
Window! '
{ 
public 
LogIn 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
DataContext   
=   
new   
LogInViewModel   ,
(  , -
)  - .
;  . /
}!! 	
}"" 
}## Û
iC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\App.xaml.cs
	namespace 	
Conqui√°nCliente
 
{ 
public 

partial 
class 
App 
: 
Application *
{ 
public 
App 
( 
) 
{ 	
this 
. 
Exit 
+= 
App_Exit !
;! "
} 	
private 
static 
void 
App_Exit $
($ %
object% +
sender, 2
,2 3
ExitEventArgs4 A
eB C
)C D
{ 	
if 
( 
PlayerSession 
. 

IsLoggedIn (
&&) +
PlayerSession, 9
.9 :
CurrentPlayer: G
!=H J
nullK O
)O P
{ 
try 
{ 
var 
loginClient #
=$ %
new& )
LoginClient* 5
(5 6
)6 7
;7 8
loginClient 
.  
SignOutPlayerAsync  2
(2 3
PlayerSession3 @
.@ A
CurrentPlayerA N
.N O
idPlayerO W
)W X
.X Y

GetAwaiterY c
(c d
)d e
.e f
	GetResultf o
(o p
)p q
;q r!
PresenceClientManager   )
.  ) *
Instance  * 2
.  2 3
Client  3 9
.  9 :
Unsubscribe  : E
(  E F
PlayerSession  F S
.  S T
CurrentPlayer  T a
.  a b
idPlayer  b j
)  j k
;  k l#
InvitationClientManager!! +
.!!+ ,

Disconnect!!, 6
(!!6 7
PlayerSession!!7 D
.!!D E
CurrentPlayer!!E R
.!!R S
idPlayer!!S [
)!![ \
;!!\ ]
PlayerSession"" !
.""! "

EndSession""" ,
("", -
)""- .
;"". /
}## 
catch$$ 
($$ 
	Exception$$  
)$$  !
{%% 
}&& 
}'' 
}(( 	
	protected)) 
override)) 
void)) 
	OnStartup))  )
())) *
StartupEventArgs))* :
e)); <
)))< =
{** 	
var++ 
langCode++ 
=++ 
Conqui√°nCliente++ *
.++* +

Properties+++ 5
.++5 6
Settings++6 >
.++> ?
Default++? F
.++F G
languageCode++G S
;++S T
Thread,, 
.,, 
CurrentThread,,  
.,,  !
CurrentUICulture,,! 1
=,,2 3
new,,4 7
System,,8 >
.,,> ?
Globalization,,? L
.,,L M
CultureInfo,,M X
(,,X Y
langCode,,Y a
),,a b
;,,b c
base-- 
.-- 
	OnStartup-- 
(-- 
e-- 
)-- 
;-- 
}.. 	
}// 
}00 Í
ÇC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Profile\UserProfilePage.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Profile &
{ 
public 

partial 
class 
UserProfilePage (
:) *
Page+ /
{ 
public 
UserProfilePage 
( 
)  
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} “
äC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Authentication\VerificationCode.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
{ 
public 

partial 
class 
VerificationCode )
:* +
Window, 2
{ 
public 
VerificationCode 
(  
)  !
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} Ÿ
ÉC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Profile\ProfileMainFrame.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
{ 
public 

partial 
class 
ProfileMainFrame )
:* +
Window, 2
{ 
private		 
static		 
ProfileMainFrame		 '
instance		( 0
;		0 1
private 
bool 
isClosed 
= 
false  %
;% &
public 
static 
Frame 
	MainFrame %
{ 	
get 
{ 
return 
GetInstance $
($ %
)% &
.& '
ProfileFrame' 3
;3 4
}5 6
} 	
public 
static 
ProfileMainFrame &
GetInstance' 2
(2 3
)3 4
{ 	
if 
( 
instance 
== 
null  
||! #
instance$ ,
., -
isClosed- 5
)5 6
{ 
instance 
= 
new 
ProfileMainFrame /
(/ 0
)0 1
;1 2
} 
return 
instance 
; 
} 	
private 
ProfileMainFrame  
(  !
)! "
{ 	
InitializeComponent 
(  
)  !
;! "
ProfileFrame 
. 
Navigate !
(! "
new" %
Profile& -
.- .
UserProfilePage. =
(= >
)> ?
)? @
;@ A
} 	
	protected!! 
override!! 
void!! 
OnClosed!!  (
(!!( )
	EventArgs!!) 2
e!!3 4
)!!4 5
{"" 	
base## 
.## 
OnClosed## 
(## 
e## 
)## 
;## 
this$$ 
.$$ 
isClosed$$ 
=$$ 
true$$  
;$$  !
}%% 	
}&& 
}'' ¿
ÑC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Authentication\SignUpData.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
{ 
public 

partial 
class 

SignUpData #
:$ %
Window& ,
{ 
public 

SignUpData 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ›	
ÄC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Authentication\SignUp.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
{ 
public 

partial 
class 
SignUp 
:  !
Window" (
{ 
public 
SignUp 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
DataContext 
= 
new 
SignUpViewModel -
(- .
). /
;/ 0
} 	
private 
void 

ClickLogIn 
(  
object  &
sender' -
,- .
RoutedEventArgs/ >
e? @
)@ A
{ 	
LogIn   
logIn   
=   
new   
LogIn   #
(  # $
)  $ %
;  % &
logIn!! 
.!! 
Show!! 
(!! 
)!! 
;!! 
this"" 
."" 
Close"" 
("" 
)"" 
;"" 
}## 	
}%% 
}&& ı
ÖC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Profile\EditProfilePicture.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Profile &
{ 
public 

partial 
class 
EditProfilePicture +
:, -
Window. 4
{		 
public

 
EditProfilePicture

 !
(

! "
)

" #
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ‡
C:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Profile\EditPassword.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Profile &
{ 
public 

partial 
class 
EditPassword %
:& '
Page( ,
{ 
public 
EditPassword 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ‡
C:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Profile\EditInfoPage.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Profile &
{ 
public 

partial 
class 
EditInfoPage %
:& '
Page( ,
{ 
public 
EditInfoPage 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} ı	
|C:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\MainMenu\MainMenu.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
MainMenu '
{ 
public 

partial 
class 
MainMenu !
:" #
Window$ *
{ 
public 
MainMenu 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
DataContext 
= 
new 
MainMenuViewModel /
(/ 0
)0 1
;1 2
} 	
private 
void "
MainMenuWindow_Closing +
(+ ,
object, 2
sender3 9
,9 :
CancelEventArgs; J
eK L
)L M
{   	
if!! 
(!! 
this!! 
.!! 
DataContext!!  
is!!! #
MainMenuViewModel!!$ 5
vm!!6 8
)!!8 9
{"" 
vm## 
.## 
OnWindowClosing## "
(##" #
)### $
;##$ %
}$$ 
}%% 	
}&& 
}'' Ó
ÄC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\MainMenu\CreateOrJoin.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
MainMenu '
{ 
public 

partial 
class 
CreateOrJoin %
:& '
Window( .
{ 
public 
CreateOrJoin 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
DataContext 
= 
new !
CreateOrJoinViewModel 3
(3 4
)4 5
;5 6
} 	
} 
} ”
ÇC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\MainMenu\ChangeLanguage.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
MainMenu '
{ 
public 

partial 
class 
ChangeLanguage '
:( )
Window* 0
{ 
public 
ChangeLanguage 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void 
ClickSpanish !
(! "
object" (
sender) /
,/ 0
RoutedEventArgs1 @
eA B
)B C
{ 	

Properties 
. 
Settings 
.  
Default  '
.' (
languageCode( 4
=5 6
$str7 >
;> ?

Properties 
. 
Settings 
.  
Default  '
.' (
Save( ,
(, -
)- .
;. /
this 
. 
DialogResult 
= 
true  $
;$ %
} 	
private!! 
void!! 
ClickEnglish!! !
(!!! "
object!!" (
sender!!) /
,!!/ 0
RoutedEventArgs!!1 @
e!!A B
)!!B C
{"" 	

Properties## 
.## 
Settings## 
.##  
Default##  '
.##' (
languageCode##( 4
=##5 6
$str##7 >
;##> ?

Properties$$ 
.$$ 
Settings$$ 
.$$  
Default$$  '
.$$' (
Save$$( ,
($$, -
)$$- .
;$$. /
this%% 
.%% 
DialogResult%% 
=%% 
true%%  $
;%%$ %
}&& 	
}'' 
}(( Å
zC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Lobby\LobbyGame.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Lobby $
{ 
public 

partial 
class 
	LobbyGame "
:# $
Window% +
{ 
public 
	LobbyGame 
( 
string 
roomCode  (
)( )
{ 	
InitializeComponent 
(  
)  !
;! "
DataContext 
= 
new 
LobbyGameViewModel 0
(0 1
roomCode1 9
)9 :
;: ;
this 
. 
Closing 
+= 
LobbyGame_Closing -
;- .
} 	
private 
void 
LobbyGame_Closing &
(& '
object' -
sender. 4
,4 5
System6 <
.< =
ComponentModel= K
.K L
CancelEventArgsL [
e\ ]
)] ^
{   	
if!! 
(!! 
DataContext!! 
is!! 
LobbyGameViewModel!! 1
vm!!2 4
)!!4 5
{"" 
if## 
(## 
vm## 
.## 
IsNavigatingAway## '
)##' (
return##) /
;##/ 0
vm$$ 
.$$ &
ShutdownApplicationCommand$$ -
.$$- .
Execute$$. 5
($$5 6
this$$6 :
)$$: ;
;$$; <
e%% 
.%% 
Cancel%% 
=%% 
true%% 
;%%  
}&& 
}'' 	
}(( 
})) Ù
ÑC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Lobby\InviteFriendsWindow.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Lobby $
{ 
public 

partial 
class 
InviteFriendsWindow ,
:- .
Window/ 5
{ 
public 
InviteFriendsWindow "
(" #
)# $
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} É
âC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Lobby\InvitationReceivedWindow.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Lobby $
{ 
public 

partial 
class $
InvitationReceivedWindow 1
:2 3
Window4 :
{ 
public $
InvitationReceivedWindow '
(' (
)( )
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
} 
} É
ÑC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\FriendList\FriendRequests.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 

FriendList )
{ 
public		 

partial		 
class		 
FriendRequests		 '
:		( )
Window		* 0
{

 
private 
readonly #
FriendRequestsViewModel 0
	viewModel1 :
;: ;
public 
FriendRequests 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
	viewModel 
= 
new #
FriendRequestsViewModel 3
(3 4
)4 5
;5 6
DataContext 
= 
	viewModel #
;# $
Loaded 
+= 
OnWindowLoaded $
;$ %
} 	
private 
async 
void 
OnWindowLoaded )
() *
object* 0
sender1 7
,7 8
RoutedEventArgs9 H
eI J
)J K
{ 	
await 
	viewModel 
. 
InitializeAsync +
(+ ,
), -
;- .
} 	
} 
} ‘
ÉC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\FriendList\FriendProfile.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 

FriendList )
{ 
public 

partial 
class 
FriendProfile &
:' (
Window) /
{		 
public

 
FriendProfile

 
(

 
	PlayerDto

 &
player

' -
,

- . 
ObservableCollection

/ C
<

C D
	SocialDto

D M
>

M N
socials

O V
)

V W
{ 	
InitializeComponent 
(  
)  !
;! "
DataContext 
= 
new "
FriendProfileViewModel 4
(4 5
player5 ;
,; <
socials= D
)D E
;E F
} 	
} 
} á
ÄC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\FriendList\FriendList.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 

FriendList )
{ 
public 

partial 
class 

FriendList #
:$ %
Window& ,
{ 
private		 
FriendListViewModel		 #
	viewModel		$ -
;		- .
public 

FriendList 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
	viewModel 
= 
new 
FriendListViewModel /
(/ 0
)0 1
;1 2
DataContext 
= 
	viewModel #
;# $
} 	
private 
async 
void 
SearchButtonClick ,
(, -
object- 3
sender4 :
,: ;
RoutedEventArgs< K
eL M
)M N
{ 	
await 
	viewModel 
. 
SearchPlayer (
(( )
txtBXSearchFriend) :
.: ;
Text; ?
)? @
;@ A
FriendsDataGrid 
. 

Visibility &
=' (

Visibility) 3
.3 4
	Collapsed4 =
;= >
SearchDataGrid 
. 

Visibility %
=& '

Visibility( 2
.2 3
Visible3 :
;: ;
} 	
} 
} ˛
òC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Authentication\PasswordRecovery\ResetPassword.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Authentication -
.- .
PasswordRecovery. >
{ 
public 

partial 
class 
ResetPassword &
:' (
Page) -
{ 
private %
PasswordRecoveryViewModel )
	viewModel* 3
;3 4
public 
ResetPassword 
( %
PasswordRecoveryViewModel 6
	viewModel7 @
)@ A
{ 	
InitializeComponent 
(  
)  !
;! "
this 
. 
	viewModel 
= 
	viewModel &
;& '
this 
. 
DataContext 
= 
this #
.# $
	viewModel$ -
;- .
} 	
private!! 
void!! "
ClickAcceptNewPassword!! +
(!!+ ,
object!!, 2
sender!!3 9
,!!9 :
RoutedEventArgs!!; J
e!!K L
)!!L M
{"" 	
if## 
(## 
	viewModel## 
!=## 
null## !
)##! "
{$$ 
	viewModel%% 
.%% 
NewPassword%% %
=%%& '
pbNewPassword%%( 5
.%%5 6
Password%%6 >
;%%> ?
	viewModel&& 
.&& 
ConfirmPassword&& )
=&&* +
pbConfirmPassword&&, =
.&&= >
Password&&> F
;&&F G
}'' 
}(( 	
})) 
}** ‰
öC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Authentication\PasswordRecovery\RequestRecovery.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Authentication -
.- .
PasswordRecovery. >
{ 
public 

partial 
class 
RequestRecovery (
:) *
Page+ /
{ 
public 
RequestRecovery 
( 
)  
{ 	
InitializeComponent 
(  
)  !
;! "
this 
. 
DataContext 
= 
new "%
PasswordRecoveryViewModel# <
(< =
)= >
;> ?
} 	
} 
} è
§C:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Authentication\PasswordRecovery\PasswordRecoveryMainFrame.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Authentication -
.- .
PasswordRecovery. >
{ 
public 

partial 
class %
PasswordRecoveryMainFrame 2
:3 4
Window5 ;
{ 
public %
PasswordRecoveryMainFrame (
(( )
)) *
{ 	
InitializeComponent 
(  
)  !
;! "
RecoveryFrame 
. 
Navigate "
(" #
new# &
RequestRecovery' 6
(6 7
)7 8
)8 9
;9 :
} 	
} 
} ‡
ôC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\View\Authentication\PasswordRecovery\CodeValidation.xaml.cs
	namespace 	
Conqui√°nCliente
 
. 
View 
. 
Authentication -
.- .
PasswordRecovery. >
{ 
public 

partial 
class 
CodeValidation '
:( )
Page* .
{ 
public 
CodeValidation 
( %
PasswordRecoveryViewModel 7
	viewModel8 A
)A B
{ 	
InitializeComponent 
(  
)  !
;! "
this 
. 
DataContext 
= 
	viewModel (
;( )
} 	
} 
} ó
xC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\ViewModelBase.cs
	namespace		 	
Conqui√°nCliente		
 
.		 
	ViewModel		 #
{

 
public 

class 
ViewModelBase 
:  "
INotifyPropertyChanged! 7
{ 
public 
event '
PropertyChangedEventHandler 0
PropertyChanged1 @
;@ A
	protected 
void 
OnPropertyChanged (
(( )
[) *
CallerMemberName* :
]: ;
string< B
propertyNameC O
=P Q
nullR V
)V W
{ 	
PropertyChanged 
? 
. 
Invoke #
(# $
this$ (
,( )
new* -$
PropertyChangedEventArgs. F
(F G
propertyNameG S
)S T
)T U
;U V
} 	
} 
} ƒ\
ÖC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Validation\SignUpValidator.cs
	namespace		 	
Conqui√°nCliente		
 
.		 
	ViewModel		 #
.		# $

Validation		$ .
{

 
public 

static 
class 
SignUpValidator '
{ 
private 
static 
readonly 
TimeSpan  (
RegexTimeout) 5
=6 7
TimeSpan8 @
.@ A
FromMillisecondsA Q
(Q R
$numR U
)U V
;V W
private 
const 
string 
NamePattern (
=) *
$str+ ;
;; <
private 
const 
string 
LastNamePattern ,
=- .
$str/ ?
;? @
private 
const 
string 
NicknamePattern ,
=- .
$str/ @
;@ A
private 
const 
string 
EmailPattern )
=* +
$str, L
;L M
private 
const 
string 
UppercasePattern -
=. /
$str0 8
;8 9
private 
const 
string 
SpecialCharPattern /
=0 1
$str2 [
;[ \
private 
const 
int 
MAX_NAME_LENGTH )
=* +
$num, .
;. /
private 
const 
int  
MAX_LAST_NAME_LENGTH .
=/ 0
$num1 3
;3 4
private 
const 
int 
MAX_NICKNAME_LENGTH -
=. /
$num0 2
;2 3
private 
const 
int 
MAX_EMAIL_LENGTH *
=+ ,
$num- /
;/ 0
private 
const 
int 
MIN_PASSWORD_LENGTH -
=. /
$num0 1
;1 2
private 
const 
int 
MAX_PASSWORD_LENGTH -
=. /
$num0 2
;2 3
private 
static 
bool 
IsMatchWithTimeout .
(. /
string/ 5
input6 ;
,; <
string= C
patternD K
)K L
{ 	
bool 
isMatch 
; 
try!! 
{"" 
isMatch## 
=## 
Regex## 
.##  
IsMatch##  '
(##' (
input##( -
,##- .
pattern##/ 6
,##6 7
RegexOptions##8 D
.##D E
None##E I
,##I J
RegexTimeout##K W
)##W X
;##X Y
}$$ 
catch%% 
(%% &
RegexMatchTimeoutException%% -
)%%- .
{&& 
isMatch'' 
='' 
false'' 
;''  
}(( 
return)) 
isMatch)) 
;)) 
}** 	
public++ 
static++ 
string++ 
ValidateName++ )
(++) *
string++* 0
name++1 5
)++5 6
{,, 	
if-- 
(-- 
string-- 
.-- 
IsNullOrEmpty-- $
(--$ %
name--% )
)--) *
)--* +
{.. 
return// 
Lang// 
.// 
ErrorNameEmpty// *
;//* +
}00 
name11 
=11 
name11 
.11 
Trim11 
(11 
)11 
;11 
if22 
(22 
name22 
.22 
Length22 
>22 
MAX_NAME_LENGTH22 -
)22- .
{33 
return44 
string44 
.44 
Format44 $
(44$ %
Lang44% )
.44) *
ErrorNameLength44* 9
,449 :
MAX_NAME_LENGTH44; J
)44J K
;44K L
}55 
if66 
(66 
!66 
IsMatchWithTimeout66 #
(66# $
name66$ (
,66( )
NamePattern66* 5
)665 6
)666 7
{77 
return88 
Lang88 
.88 
ErrorValidName88 *
;88* +
}99 
return;; 
string;; 
.;; 
Empty;; 
;;;  
}<< 	
public>> 
static>> 
string>> 
ValidateLastName>> -
(>>- .
string>>. 4
lastName>>5 =
)>>= >
{?? 	
if@@ 
(@@ 
string@@ 
.@@ 
IsNullOrEmpty@@ $
(@@$ %
lastName@@% -
)@@- .
)@@. /
{AA 
returnBB 
LangBB 
.BB 
ErrorLastNameEmptyBB .
;BB. /
}CC 
lastNameDD 
=DD 
lastNameDD 
.DD  
TrimDD  $
(DD$ %
)DD% &
;DD& '
ifFF 
(FF 
lastNameFF 
.FF 
LengthFF 
>FF  ! 
MAX_LAST_NAME_LENGTHFF" 6
)FF6 7
{GG 
returnHH 
stringHH 
.HH 
FormatHH $
(HH$ %
LangHH% )
.HH) *
ErrorLastNameLengthHH* =
,HH= > 
MAX_LAST_NAME_LENGTHHH? S
)HHS T
;HHT U
}II 
ifKK 
(KK 
!KK 
IsMatchWithTimeoutKK #
(KK# $
lastNameKK$ ,
,KK, -
LastNamePatternKK. =
)KK= >
)KK> ?
{LL 
returnMM 
LangMM 
.MM %
ErrorLastNameInvalidCharsMM 5
;MM5 6
}NN 
returnPP 
stringPP 
.PP 
EmptyPP 
;PP  
}QQ 	
publicSS 
staticSS 
stringSS 
ValidateNicknameSS -
(SS- .
stringSS. 4
nicknameSS5 =
)SS= >
{TT 	
ifUU 
(UU 
stringUU 
.UU 
IsNullOrEmptyUU $
(UU$ %
nicknameUU% -
)UU- .
)UU. /
{VV 
returnWW 
LangWW 
.WW 
ErrorNicknameEmptyWW .
;WW. /
}XX 
nicknameYY 
=YY 
nicknameYY 
.YY  
TrimYY  $
(YY$ %
)YY% &
;YY& '
ifZZ 
(ZZ 
nicknameZZ 
.ZZ 
LengthZZ 
>ZZ  !
MAX_NICKNAME_LENGTHZZ" 5
)ZZ5 6
{[[ 
return\\ 
string\\ 
.\\ 
Format\\ $
(\\$ %
Lang\\% )
.\\) *
ErrorNicknameLength\\* =
,\\= >
MAX_NICKNAME_LENGTH\\? R
)\\R S
;\\S T
}]] 
if__ 
(__ 
!__ 
IsMatchWithTimeout__ #
(__# $
nickname__$ ,
,__, -
NicknamePattern__. =
)__= >
)__> ?
{`` 
returnaa 
Langaa 
.aa %
ErrorNicknameInvalidCharsaa 5
;aa5 6
}bb 
returndd 
stringdd 
.dd 
Emptydd 
;dd  
}ee 	
publicgg 
staticgg 
stringgg 
ValidateEmailgg *
(gg* +
stringgg+ 1
emailgg2 7
)gg7 8
{hh 	
ifii 
(ii 
stringii 
.ii 
IsNullOrWhiteSpaceii )
(ii) *
emailii* /
)ii/ 0
)ii0 1
{jj 
returnkk 
Langkk 
.kk 
ErrorEmailEmptykk +
;kk+ ,
}ll 
emailmm 
=mm 
emailmm 
.mm 
Trimmm 
(mm 
)mm  
;mm  !
ifnn 
(nn 
emailnn 
.nn 
Lengthnn 
>nn 
MAX_EMAIL_LENGTHnn /
)nn/ 0
{oo 
returnpp 
stringpp 
.pp 
Formatpp $
(pp$ %
Langpp% )
.pp) *
ErrorEmailLenghtpp* :
,pp: ;
MAX_EMAIL_LENGTHpp< L
)ppL M
;ppM N
}qq 
ifss 
(ss 
!ss 
IsMatchWithTimeoutss #
(ss# $
emailss$ )
,ss) *
EmailPatternss+ 7
)ss7 8
)ss8 9
{tt 
returnuu 
Languu 
.uu #
ErrorEmailInvalidFormatuu 3
;uu3 4
}vv 
returnxx 
stringxx 
.xx 
Emptyxx 
;xx  
}yy 	
public{{ 
static{{ 
string{{ 
ValidatePassword{{ -
({{- .
string{{. 4
password{{5 =
){{= >
{|| 	
if}} 
(}} 
string}} 
.}} 
IsNullOrEmpty}} $
(}}$ %
password}}% -
)}}- .
)}}. /
{~~ 
return 
Lang 
. 
ErrorPasswordEmpty .
;. /
}
ÄÄ 
if
ÇÇ 
(
ÇÇ 
password
ÇÇ 
.
ÇÇ 
Length
ÇÇ 
<
ÇÇ  !!
MIN_PASSWORD_LENGTH
ÇÇ" 5
||
ÇÇ6 8
password
ÇÇ9 A
.
ÇÇA B
Length
ÇÇB H
>
ÇÇI J!
MAX_PASSWORD_LENGTH
ÇÇK ^
)
ÇÇ^ _
{
ÉÉ 
return
ÑÑ 
string
ÑÑ 
.
ÑÑ 
Format
ÑÑ $
(
ÑÑ$ %
Lang
ÑÑ% )
.
ÑÑ) *!
ErrorPasswordLength
ÑÑ* =
,
ÑÑ= >!
MIN_PASSWORD_LENGTH
ÑÑ? R
,
ÑÑR S!
MAX_PASSWORD_LENGTH
ÑÑT g
)
ÑÑg h
;
ÑÑh i
}
ÖÖ 
if
áá 
(
áá 
password
áá 
.
áá 
Contains
áá !
(
áá! "
$str
áá" %
)
áá% &
)
áá& '
{
àà 
return
ââ 
Lang
ââ 
.
ââ #
ErrorPasswordNoSpaces
ââ 1
;
ââ1 2
}
ää 
if
åå 
(
åå 
!
åå  
IsMatchWithTimeout
åå #
(
åå# $
password
åå$ ,
,
åå, -
UppercasePattern
åå. >
)
åå> ?
)
åå? @
{
çç 
return
éé 
Lang
éé 
.
éé &
ErrorPasswordNoUppercase
éé 4
;
éé4 5
}
èè 
if
ëë 
(
ëë 
!
ëë  
IsMatchWithTimeout
ëë #
(
ëë# $
password
ëë$ ,
,
ëë, - 
SpecialCharPattern
ëë. @
)
ëë@ A
)
ëëA B
{
íí 
return
ìì 
Lang
ìì 
.
ìì (
ErrorPasswordNoSpecialChar
ìì 6
;
ìì6 7
}
îî 
return
ññ 
string
ññ 
.
ññ 
Empty
ññ 
;
ññ  
}
óó 	
public
òò 
static
òò 
string
òò %
ValidateConfirmPassword
òò 4
(
òò4 5
string
òò5 ;
password
òò< D
,
òòD E
string
òòF L
confirmPassword
òòM \
)
òò\ ]
{
ôô 	
if
öö 
(
öö 
password
öö 
!=
öö 
confirmPassword
öö +
)
öö+ ,
{
õõ 
return
úú 
Lang
úú 
.
úú #
ErrorPasswordMismatch
úú 1
;
úú1 2
}
ùù 
return
ûû 
string
ûû 
.
ûû 
Empty
ûû 
;
ûû  
}
üü 	
}
†† 
}°° †
èC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Validation\PasswordRecoveryValidator.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $

Validation$ .
{ 
public 

static 
class %
PasswordRecoveryValidator 1
{ 
public		 
static		 
string		 
ValidateEmail		 *
(		* +
string		+ 1
email		2 7
)		7 8
{

 	
return 
SignUpValidator "
." #
ValidateEmail# 0
(0 1
email1 6
)6 7
;7 8
} 	
public 
static 
string 
ValidateToken *
(* +
string+ 1
code2 6
)6 7
{ 	
if 
( 
string 
. 
IsNullOrWhiteSpace )
() *
code* .
). /
||0 2
code3 7
.7 8
Length8 >
!=? A
$numB C
||D F
!G H
intH K
.K L
TryParseL T
(T U
codeU Y
,Y Z
out[ ^
__ `
)` a
)a b
{ 
return 
Lang 
. *
ErrorVerificationCodeIncorrect :
;: ;
} 
return 
string 
. 
Empty 
;  
} 	
public 
static 
string 
ValidatePasswords .
(. /
string/ 5
password6 >
,> ?
string@ F
confirmPasswordG V
)V W
{ 	
string 
passwordError  
=! "
SignUpValidator# 2
.2 3
ValidatePassword3 C
(C D
passwordD L
)L M
;M N
if 
( 
! 
string 
. 
IsNullOrEmpty %
(% &
passwordError& 3
)3 4
)4 5
{ 
return 
passwordError $
;$ %
} 
string 
mismatchError  
=! "
SignUpValidator# 2
.2 3#
ValidateConfirmPassword3 J
(J K
passwordK S
,S T
confirmPasswordU d
)d e
;e f
if   
(   
!   
string   
.   
IsNullOrEmpty   %
(  % &
mismatchError  & 3
)  3 4
)  4 5
{!! 
return"" 
mismatchError"" $
;""$ %
}## 
return$$ 
string$$ 
.$$ 
Empty$$ 
;$$  
}%% 	
}&& 
}'' ◊
ÑC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Validation\LogInValidator.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $

Validation$ .
{		 
public

 

static

 
class

 
LogInValidator

 &
{ 
public 
static 
string 
ValidateEmail *
(* +
string+ 1
email2 7
)7 8
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
email% *
)* +
)+ ,
{ 
return 
Lang 
. 
ErrorEmailEmpty +
;+ ,
} 
try 
{ 
var 
addr 
= 
new 
System %
.% &
Net& )
.) *
Mail* .
.. /
MailAddress/ :
(: ;
email; @
)@ A
;A B
if 
( 
addr 
. 
Address  
!=! #
email$ )
)) *
{ 
return 
Lang 
.  #
ErrorEmailInvalidFormat  7
;7 8
} 
} 
catch 
{ 
return 
Lang 
. #
ErrorEmailInvalidFormat 3
;3 4
} 
return 
string 
. 
Empty 
;  
} 	
public!! 
static!! 
string!! 
ValidatePassword!! -
(!!- .
string!!. 4
password!!5 =
)!!= >
{"" 	
if## 
(## 
string## 
.## 
IsNullOrEmpty## $
(##$ %
password##% -
)##- .
)##. /
{$$ 
return%% 
Lang%% 
.%% 
ErrorPasswordEmpty%% .
;%%. /
}&& 
return'' 
string'' 
.'' 
Empty'' 
;''  
}(( 	
})) 
}** π
wC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\RelayCommand.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
{		 
public

 

class

 
RelayCommand

 
:

 
ICommand

  (
{ 
private 
readonly 
Action 
<  
object  &
>& '
execute( /
;/ 0
private 
readonly 
	Predicate "
<" #
object# )
>) *

canExecute+ 5
;5 6
public 
RelayCommand 
( 
Action "
<" #
object# )
>) *
execute+ 2
,2 3
	Predicate4 =
<= >
object> D
>D E

canExecuteF P
=Q R
nullS W
)W X
{ 	
this 
. 
execute 
= 
execute "
??# %
throw& +
new, /!
ArgumentNullException0 E
(E F
nameofF L
(L M
executeM T
)T U
)U V
;V W
this 
. 

canExecute 
= 

canExecute (
;( )
} 	
public 
event 
EventHandler !
CanExecuteChanged" 3
{ 	
add 
{ 
CommandManager  
.  !
RequerySuggested! 1
+=2 4
value5 :
;: ;
}< =
remove 
{ 
CommandManager #
.# $
RequerySuggested$ 4
-=5 7
value8 =
;= >
}? @
} 	
public 
bool 

CanExecute 
( 
object %
	parameter& /
)/ 0
{ 	
return 

canExecute 
==  
null! %
||& (

canExecute) 3
(3 4
	parameter4 =
)= >
;> ?
} 	
public   
void   
Execute   
(   
object   "
	parameter  # ,
)  , -
{!! 	
execute"" 
("" 
	parameter"" 
)"" 
;"" 
}## 	
}$$ 
}&& ¡0
éC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Profile\EditProfilePictureViewModel.cs
	namespace		 	
Conqui√°nCliente		
 
.		 
	ViewModel		 #
.		# $
Profile		$ +
{

 
public 

class '
EditProfilePictureViewModel ,
:- .
ViewModelBase/ <
{ 
private 
string 
selectedImagePath (
;( )
public 
string 
SelectedImagePath '
{ 	
get 
{ 
return 
selectedImagePath *
;* +
}, -
set 
{ 
selectedImagePath !
=" #
value$ )
;) *
OnPropertyChanged !
(! "
nameof" (
(( )
SelectedImagePath) :
): ;
); <
;< =
} 
} 	
public 
string %
CurrentProfilePicturePath /
{0 1
get2 5
;5 6
}7 8
public 
ICommand 
SelectImageCommand *
{+ ,
get- 0
;0 1
}2 3
public 
ICommand '
ChangeProfilePictureCommand 3
{4 5
get6 9
;9 :
}; <
public 
ICommand 
CloseWindowCommand *
{+ ,
get- 0
;0 1
}2 3
public '
EditProfilePictureViewModel *
(* +
)+ ,
{ 	%
CurrentProfilePicturePath   %
=  & '
PlayerSession  ( 5
.  5 6
CurrentPlayer  6 C
.  C D
	pathPhoto  D M
;  M N
SelectImageCommand!! 
=!!  
new!!! $
RelayCommand!!% 1
(!!1 2
ExecuteSelectImage!!2 D
)!!D E
;!!E F'
ChangeProfilePictureCommand## '
=##( )
new##* -
RelayCommand##. :
(##: ;'
ExecuteChangeProfilePicture##; V
,##V W*
CanExecuteChangeProfilePicture##X v
)##v w
;##w x
CloseWindowCommand%% 
=%%  
new%%! $
RelayCommand%%% 1
(%%1 2
ExecuteCloseWindow%%2 D
)%%D E
;%%E F
}&& 	
private'' 
void'' 
ExecuteSelectImage'' '
(''' (
object''( .
	parameter''/ 8
)''8 9
{(( 	
SelectedImagePath)) 
=)) 
	parameter))  )
as))* ,
string))- 3
;))3 4
}** 	
private,, 
bool,, *
CanExecuteChangeProfilePicture,, 3
(,,3 4
object,,4 :
obj,,; >
),,> ?
{-- 	
return.. 
!.. 
string.. 
... 
IsNullOrEmpty.. (
(..( )
SelectedImagePath..) :
)..: ;
;..; <
}// 	
private11 
async11 
void11 '
ExecuteChangeProfilePicture11 6
(116 7
object117 =
obj11> A
)11A B
{22 	
PlayerSession33 
.33  
UpdateProfilePicture33 .
(33. /
SelectedImagePath33/ @
)33@ A
;33A B
try55 
{66 
var88 
userProfileClient88 %
=88& '
new88( +
UserProfileClient88, =
(88= >
)88> ?
;88? @
int:: 
playerId:: 
=:: 
PlayerSession:: ,
.::, -
CurrentPlayer::- :
.::: ;
idPlayer::; C
;::C D
bool<< 
success<< 
=<< 
await<< $
userProfileClient<<% 6
.<<6 7%
UpdateProfilePictureAsync<<7 P
(<<P Q
playerId<<Q Y
,<<Y Z
SelectedImagePath<<[ l
)<<l m
;<<m n
if>> 
(>> 
success>> 
)>> 
{?? 
ExecuteCloseWindow@@ &
(@@& '
obj@@' *
)@@* +
;@@+ ,
}AA 
elseBB 
{CC 

MessageBoxDD 
.DD 
ShowDD #
(DD# $
LangDD$ (
.DD( )
ErrorUpdatePhotoDD) 9
,DD9 :
LangDD; ?
.DD? @

TitleErrorDD@ J
)DDJ K
;DDK L
}EE 
}FF 
catchGG 
(GG %
EndpointNotFoundExceptionGG ,
)GG, -
{HH 

MessageBoxII 
.II 
ShowII 
(II  
LangII  $
.II$ %"
ErrorServerUnavailableII% ;
,II; <
LangII= A
.IIA B 
TitleConnectionErrorIIB V
)IIV W
;IIW X
}JJ 
catchKK 
(KK 
SystemKK 
.KK 
	ExceptionKK #
exKK$ &
)KK& '
{LL 

MessageBoxMM 
.MM 
ShowMM 
(MM  
stringMM  &
.MM& '
FormatMM' -
(MM- .
LangMM. 2
.MM2 3
ErrorUnexpectedMM3 B
,MMB C
exMMD F
.MMF G
MessageMMG N
)MMN O
,MMO P
LangMMQ U
.MMU V

TitleErrorMMV `
)MM` a
;MMa b
}NN 
}OO 	
privateQQ 
voidQQ 
ExecuteCloseWindowQQ '
(QQ' (
objectQQ( .
	parameterQQ/ 8
)QQ8 9
{RR 	
foreachSS 
(SS 
WindowSS 
windowSS "
inSS# %
ApplicationSS& 1
.SS1 2
CurrentSS2 9
.SS9 :
WindowsSS: A
)SSA B
{TT 
ifUU 
(UU 
windowUU 
.UU 
DataContextUU &
==UU' )
thisUU* .
)UU. /
{VV 
windowWW 
.WW 
CloseWW  
(WW  !
)WW! "
;WW" #
breakXX 
;XX 
}YY 
}ZZ 
}[[ 	
}\\ 
}]] ¨k
áC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Profile\UserProfileViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
Profile$ +
{ 
public 

class  
UserProfileViewModel %
:& '
ViewModelBase( 5
{ 
private 
string 
profileImagePath '
;' (
private 
	PlayerDto 
fullPlayerProfile +
;+ ,
public 
string 
ProfileImagePath &
{ 	
get 
=> 
profileImagePath #
;# $
set 
{ 
profileImagePath "
=# $
value% *
;* +
OnPropertyChanged, =
(= >
)> ?
;? @
}A B
} 	
private 
string 
nickname 
;  
public 
string 
Nickname 
{ 	
get 
=> 
nickname 
; 
set 
{ 
nickname 
= 
value "
;" #
OnPropertyChanged$ 5
(5 6
)6 7
;7 8
}9 :
}   	
private"" 
string"" 
email"" 
;"" 
public## 
string## 
Email## 
{$$ 	
get%% 
=>%% 
email%% 
;%% 
set&& 
{&& 
email&& 
=&& 
value&& 
;&&  
OnPropertyChanged&&! 2
(&&2 3
)&&3 4
;&&4 5
}&&6 7
}'' 	
private)) 
string)) 
name)) 
;)) 
public** 
string** 
Name** 
{++ 	
get,, 
=>,, 
name,, 
;,, 
set-- 
{-- 
name-- 
=-- 
value-- 
;-- 
OnPropertyChanged--  1
(--1 2
)--2 3
;--3 4
}--5 6
}.. 	
private00 
string00 
lastName00 
;00  
public11 
string11 
LastName11 
{22 	
get33 
=>33 
lastName33 
;33 
set44 
{44 
lastName44 
=44 
value44 "
;44" #
OnPropertyChanged44$ 5
(445 6
)446 7
;447 8
}449 :
}55 	
private77 
string77 
level77 
;77 
public88 
string88 
Level88 
{99 	
get:: 
=>:: 
level:: 
;:: 
set;; 
{;; 
level;; 
=;; 
value;; 
;;;  
OnPropertyChanged;;! 2
(;;2 3
);;3 4
;;;4 5
};;6 7
}<< 	
private>> 
string>> 
facebook>> 
;>>  
public?? 
string?? 
Facebook?? 
{@@ 	
getAA 
=>AA 
facebookAA 
;AA 
setBB 
{BB 
facebookBB 
=BB 
valueBB "
;BB" #
OnPropertyChangedBB$ 5
(BB5 6
)BB6 7
;BB7 8
}BB9 :
}CC 	
privateEE 
stringEE 
	instagramEE  
;EE  !
publicFF 
stringFF 
	InstagramFF 
{GG 	
getHH 
=>HH 
	instagramHH 
;HH 
setII 
{II 
	instagramII 
=II 
valueII #
;II# $
OnPropertyChangedII% 6
(II6 7
)II7 8
;II8 9
}II: ;
}JJ 	
publicMM 
ICommandMM !
NavigateToEditCommandMM -
{MM. /
getMM0 3
;MM3 4
}MM5 6
publicNN 
ICommandNN 
NavigateBackCommandNN +
{NN, -
getNN. 1
;NN1 2
}NN3 4
publicOO 
ICommandOO /
#NavigateToEditProfilePictureCommandOO ;
{OO< =
getOO> A
;OOA B
}OOC D
publicQQ  
UserProfileViewModelQQ #
(QQ# $
)QQ$ %
{RR 	
NavigateBackCommandSS 
=SS  !
newSS" %
RelayCommandSS& 2
(SS2 3
ExecuteNavigateBackSS3 F
)SSF G
;SSG H!
NavigateToEditCommandTT !
=TT" #
newTT$ '
RelayCommandTT( 4
(TT4 5!
ExecuteNavigateToEditTT5 J
)TTJ K
;TTK L/
#NavigateToEditProfilePictureCommandUU /
=UU0 1
newUU2 5
RelayCommandUU6 B
(UUB C/
#ExecuteNavigateToEditProfilePictureUUC f
)UUf g
;UUg h
_VV 
=VV 
LoadPlayerDataVV 
(VV 
)VV  
;VV  !
}WW 	
privateYY 
asyncYY 
TaskYY 
LoadPlayerDataYY )
(YY) *
)YY* +
{ZZ 	
if[[ 
([[ 
PlayerSession[[ 
.[[ 

IsLoggedIn[[ (
)[[( )
{\\ 
var]] 
sessionPlayer]] !
=]]" #
PlayerSession]]$ 1
.]]1 2
CurrentPlayer]]2 ?
;]]? @
Nickname^^ 
=^^ 
sessionPlayer^^ (
.^^( )
nickname^^) 1
;^^1 2
string`` 
initialImageName`` '
=``( )
System``* 0
.``0 1
IO``1 3
.``3 4
Path``4 8
.``8 9
GetFileName``9 D
(``D E
sessionPlayer``E R
.``R S
	pathPhoto``S \
)``\ ]
;``] ^
SetProfileImageaa 
(aa  
initialImageNameaa  0
)aa0 1
;aa1 2
trycc 
{dd 
varee 
userProfileClientee )
=ee* +
newee, /
UserProfileClientee0 A
(eeA B
)eeB C
;eeC D
fullPlayerProfilegg %
=gg& '
awaitgg( -
userProfileClientgg. ?
.gg? @
GetPlayerByIdAsyncgg@ R
(ggR S
sessionPlayerggS `
.gg` a
idPlayergga i
)ggi j
;ggj k
ifii 
(ii 
fullPlayerProfileii )
.ii) *
idPlayerii* 2
>ii3 4
$numii5 6
)ii6 7
{jj 
Emailkk 
=kk 
fullPlayerProfilekk  1
.kk1 2
emailkk2 7
;kk7 8
Namell 
=ll 
fullPlayerProfilell 0
.ll0 1
namell1 5
;ll5 6
LastNamemm  
=mm! "
fullPlayerProfilemm# 4
.mm4 5
lastNamemm5 =
;mm= >
Levelnn 
=nn 
fullPlayerProfilenn  1
.nn1 2
levelnn2 7
?nn7 8
.nn8 9
ToStringnn9 A
(nnA B
)nnB C
??nnD F
$strnnG J
;nnJ K
stringpp 
serverImageNamepp .
=pp/ 0
Systempp1 7
.pp7 8
IOpp8 :
.pp: ;
Pathpp; ?
.pp? @
GetFileNamepp@ K
(ppK L
fullPlayerProfileppL ]
.pp] ^
	pathPhotopp^ g
)ppg h
;pph i
SetProfileImageqq '
(qq' (
serverImageNameqq( 7
)qq7 8
;qq8 9
PlayerSessionss %
.ss% &
UpdateSessionss& 3
(ss3 4
fullPlayerProfiless4 E
)ssE F
;ssF G
}tt 
varvv 
socialsvv 
=vv  !
awaitvv" '
userProfileClientvv( 9
.vv9 :!
GetPlayerSocialsAsyncvv: O
(vvO P
sessionPlayervvP ]
.vv] ^
idPlayervv^ f
)vvf g
;vvg h
ifww 
(ww 
socialsww 
.ww  
Anyww  #
(ww# $
)ww$ %
)ww% &
{xx 
Facebookyy  
=yy! "
socialsyy# *
.yy* +
FirstOrDefaultyy+ 9
(yy9 :
syy: ;
=>yy< >
syy? @
.yy@ A
IdSocialTypeyyA M
==yyN P
$numyyQ R
)yyR S
?yyS T
.yyT U
UserLinkyyU ]
;yy] ^
	Instagramzz !
=zz" #
socialszz$ +
.zz+ ,
FirstOrDefaultzz, :
(zz: ;
szz; <
=>zz= ?
szz@ A
.zzA B
IdSocialTypezzB N
==zzO Q
$numzzR S
)zzS T
?zzT U
.zzU V
UserLinkzzV ^
;zz^ _
}{{ 
}}} 
catch~~ 
(~~ %
EndpointNotFoundException~~ 0
)~~0 1
{ 

MessageBox
ÄÄ 
.
ÄÄ 
Show
ÄÄ #
(
ÄÄ# $
Lang
ÄÄ$ (
.
ÄÄ( )$
ErrorServerUnavailable
ÄÄ) ?
,
ÄÄ? @
Lang
ÄÄA E
.
ÄÄE F"
TitleConnectionError
ÄÄF Z
)
ÄÄZ [
;
ÄÄ[ \
}
ÅÅ 
catch
ÇÇ 
(
ÇÇ 
System
ÇÇ 
.
ÇÇ 
	Exception
ÇÇ '
ex
ÇÇ( *
)
ÇÇ* +
{
ÉÉ 

MessageBox
ÑÑ 
.
ÑÑ 
Show
ÑÑ #
(
ÑÑ# $
string
ÑÑ$ *
.
ÑÑ* +
Format
ÑÑ+ 1
(
ÑÑ1 2
Lang
ÑÑ2 6
.
ÑÑ6 7
ErrorUnexpected
ÑÑ7 F
,
ÑÑF G
ex
ÑÑH J
.
ÑÑJ K
Message
ÑÑK R
)
ÑÑR S
,
ÑÑS T
Lang
ÑÑU Y
.
ÑÑY Z

TitleError
ÑÑZ d
)
ÑÑd e
;
ÑÑe f
}
ÖÖ 
}
ÜÜ 
}
áá 	
private
ââ 
void
ââ 
SetProfileImage
ââ $
(
ââ$ %
string
ââ% +
	imageName
ââ, 5
)
ââ5 6
{
ää 	
if
ãã 
(
ãã 
!
ãã 
string
ãã 
.
ãã 
IsNullOrEmpty
ãã %
(
ãã% &
	imageName
ãã& /
)
ãã/ 0
)
ãã0 1
{
åå 
string
çç 
fullPath
çç 
=
çç  !
$"
çç" $
$str
çç$ R
{
ççR S
	imageName
ççS \
}
çç\ ]
"
çç] ^
;
çç^ _
ProfileImagePath
éé  
=
éé! "
fullPath
éé# +
;
éé+ ,
}
èè 
}
êê 	
private
ìì 
static
ìì 
void
ìì !
ExecuteNavigateBack
ìì /
(
ìì/ 0
object
ìì0 6
	parameter
ìì7 @
)
ìì@ A
{
îî 	
var
ïï 
mainMenu
ïï 
=
ïï 
new
ïï 
View
ïï #
.
ïï# $
MainMenu
ïï$ ,
.
ïï, -
MainMenu
ïï- 5
(
ïï5 6
)
ïï6 7
;
ïï7 8
mainMenu
ññ 
.
ññ 
Show
ññ 
(
ññ 
)
ññ 
;
ññ 
if
óó 
(
óó 
	parameter
óó 
is
óó 
Page
óó !
currentPage
óó" -
)
óó- .
{
òò 
Window
ôô 
parentWindow
ôô #
=
ôô$ %
Window
ôô& ,
.
ôô, -
	GetWindow
ôô- 6
(
ôô6 7
currentPage
ôô7 B
)
ôôB C
;
ôôC D
parentWindow
öö 
?
öö 
.
öö 
Close
öö #
(
öö# $
)
öö$ %
;
öö% &
}
õõ 
}
úú 	
private
ùù 
void
ùù #
ExecuteNavigateToEdit
ùù *
(
ùù* +
object
ùù+ 1
	parameter
ùù2 ;
)
ùù; <
{
ûû 	
var
üü 
editInfoViewModel
üü !
=
üü" #
new
üü$ '
EditInfoViewModel
üü( 9
(
üü9 :
fullPlayerProfile
üü: K
)
üüK L
;
üüL M
var
°° 
editInfoPage
°° 
=
°° 
new
°° "
EditInfoPage
°°# /
{
¢¢ 
DataContext
££ 
=
££ 
editInfoViewModel
££ /
}
§§ 
;
§§ 
ProfileMainFrame
¶¶ 
.
¶¶ 
	MainFrame
¶¶ &
.
¶¶& '
Navigate
¶¶' /
(
¶¶/ 0
editInfoPage
¶¶0 <
)
¶¶< =
;
¶¶= >
}
ßß 	
private
®® 
void
®® 1
#ExecuteNavigateToEditProfilePicture
®® 8
(
®®8 9
object
®®9 ?
obj
®®@ C
)
®®C D
{
©© 	 
EditProfilePicture
™™  
editProfilePicture
™™ 1
=
™™2 3
new
™™4 7 
EditProfilePicture
™™8 J
(
™™J K
)
™™K L
;
™™L M 
editProfilePicture
´´ 
.
´´ 

ShowDialog
´´ )
(
´´) *
)
´´* +
;
´´+ ,
if
≠≠ 
(
≠≠ 
PlayerSession
≠≠ 
.
≠≠ 

IsLoggedIn
≠≠ (
)
≠≠( )
{
ÆÆ 
string
ØØ 
serverImageName
ØØ &
=
ØØ' (
System
ØØ) /
.
ØØ/ 0
IO
ØØ0 2
.
ØØ2 3
Path
ØØ3 7
.
ØØ7 8
GetFileName
ØØ8 C
(
ØØC D
PlayerSession
ØØD Q
.
ØØQ R
CurrentPlayer
ØØR _
.
ØØ_ `
	pathPhoto
ØØ` i
)
ØØi j
;
ØØj k
SetProfileImage
∞∞ 
(
∞∞  
serverImageName
∞∞  /
)
∞∞/ 0
;
∞∞0 1
}
±± 
}
≤≤ 	
}
≥≥ 
}¥¥ ÕÜ
ÑC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Profile\EditInfoViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
Profile$ +
{ 
public 

class 
EditInfoViewModel "
:# $
ViewModelBase% 2
{ 
private 
bool 
	isLoading 
; 
public 
bool 
	IsLoading 
{ 	
get 
=> 
	isLoading 
; 
set 
{ 
	isLoading 
= 
value !
;! "
OnPropertyChanged !
(! "
nameof" (
(( )
	IsLoading) 2
)2 3
)3 4
;4 5
CommandManager 
. &
InvalidateRequerySuggested 9
(9 :
): ;
;; <
} 
} 	
private   
	PlayerDto   
player    
;    !
public!! 
	PlayerDto!! 
Player!! 
{"" 	
get## 
=>## 
player## 
;## 
set$$ 
{$$ 
player$$ 
=$$ 
value$$  
;$$  !
OnPropertyChanged$$" 3
($$3 4
)$$4 5
;$$5 6
}$$7 8
}%% 	
private'' 
string'' 
instagramLink'' $
;''$ %
public(( 
string(( 
InstagramLink(( #
{)) 	
get** 
=>** 
instagramLink**  
;**  !
set++ 
{++ 
instagramLink++ 
=++  !
value++" '
;++' (
OnPropertyChanged++) :
(++: ;
)++; <
;++< =
}++> ?
},, 	
private.. 
string.. 
facebookLink.. #
;..# $
public// 
string// 
FacebookLink// "
{00 	
get11 
=>11 
facebookLink11 
;11  
set22 
{22 
facebookLink22 
=22  
value22! &
;22& '
OnPropertyChanged22( 9
(229 :
)22: ;
;22; <
}22= >
}33 	
public55 
ICommand55 
SaveChangesCommand55 *
{55+ ,
get55- 0
;550 1
}552 3
public66 
ICommand66 
CancelCommand66 %
{66& '
get66( +
;66+ ,
}66- .
public77 
ICommand77 +
NavigateToChangePasswordCommand77 7
{778 9
get77: =
;77= >
}77? @
public88 
EditInfoViewModel88  
(88  !
	PlayerDto88! *
	playerDto88+ 4
)884 5
{99 	
Player:: 
=:: 
	playerDto:: 
;:: 
SaveChangesCommand;; 
=;;  
new;;! $
RelayCommand;;% 1
(;;1 2
ExecuteSaveChanges;;2 D
,;;D E!
CanExecuteSaveChanges;;F [
);;[ \
;;;\ ]+
NavigateToChangePasswordCommand<< +
=<<, -
new<<. 1
RelayCommand<<2 >
(<<> ?+
ExecuteNavigateToChangePassword<<? ^
,<<^ _$
CanExecuteChangePassword<<` x
)<<x y
;<<y z
CancelCommand== 
=== 
new== 
RelayCommand==  ,
(==, -
ExecuteCancel==- :
)==: ;
;==; <
LoadPlayerSocials>> 
(>> 
)>> 
;>>  
}?? 	
privateAA 
boolAA $
CanExecuteChangePasswordAA -
(AA- .
objectAA. 4
	parameterAA5 >
)AA> ?
{BB 	
returnCC 
!CC 
	IsLoadingCC 
;CC 
}DD 	
privateFF 
asyncFF 
voidFF +
ExecuteNavigateToChangePasswordFF :
(FF: ;
objectFF; A
	parameterFFB K
)FFK L
{GG 	
tryHH 
{II 
	IsLoadingJJ 
=JJ 
trueJJ  
;JJ  !
varLL 

passwordVMLL 
=LL  
newLL! $%
PasswordRecoveryViewModelLL% >
(LL> ?
)LL? @
;LL@ A

passwordVMMM 
.MM 
EmailMM  
=MM! "
PlayerSessionMM# 0
.MM0 1
CurrentPlayerMM1 >
.MM> ?
emailMM? D
;MMD E

passwordVMNN 
.NN 
IsEditProfileFlowNN ,
=NN- .
trueNN/ 3
;NN3 4
boolPP 
successPP 
=PP 
awaitPP $

passwordVMPP% /
.PP/ 0+
RequestChangePasswordTokenAsyncPP0 O
(PPO P
)PPP Q
;PPQ R
ifQQ 
(QQ 
successQQ 
)QQ 
{RR 
varSS 
pageSS 
=SS 
	parameterSS (
asSS) +
PageSS, 0
;SS0 1
pageTT 
?TT 
.TT 
NavigationServiceTT +
?TT+ ,
.TT, -
NavigateTT- 5
(TT5 6
newTT6 9
CodeValidationTT: H
(TTH I

passwordVMTTI S
)TTS T
)TTT U
;TTU V
}UU 
}VV 
catchWW 
(WW 
	ExceptionWW 
exWW 
)WW  
{XX 

MessageBoxYY 
.YY 
ShowYY 
(YY  
exYY  "
.YY" #
MessageYY# *
,YY* +
$strYY, 3
)YY3 4
;YY4 5
}ZZ 
finally[[ 
{\\ 
	IsLoading]] 
=]] 
false]] !
;]]! "
}^^ 
}__ 	
privateaa 
voidaa 
LoadPlayerSocialsaa &
(aa& '
)aa' (
{bb 	
trycc 
{dd 
varee 
clientee 
=ee 
newee  
UserProfileClientee! 2
(ee2 3
)ee3 4
;ee4 5
	SocialDtoff 
[ff 
]ff 
socialsArrayff (
=ff) *
clientff+ 1
.ff1 2
GetPlayerSocialsff2 B
(ffB C
PlayerffC I
.ffI J
idPlayerffJ R
)ffR S
;ffS T
Listgg 
<gg 
	SocialDtogg 
>gg 
socialsgg  '
=gg( )
socialsArraygg* 6
?gg6 7
.gg7 8
ToListgg8 >
(gg> ?
)gg? @
??ggA C
newggD G
ListggH L
<ggL M
	SocialDtoggM V
>ggV W
(ggW X
)ggX Y
;ggY Z
InstagramLinkii 
=ii 
socialsii  '
.ii' (
FirstOrDefaultii( 6
(ii6 7
sii7 8
=>ii9 ;
sii< =
.ii= >
IdSocialTypeii> J
==iiK M
$numiiN O
)iiO P
?iiP Q
.iiQ R
UserLinkiiR Z
??ii[ ]
$strii^ `
;ii` a
FacebookLinkjj 
=jj 
socialsjj &
.jj& '
FirstOrDefaultjj' 5
(jj5 6
sjj6 7
=>jj8 :
sjj; <
.jj< =
IdSocialTypejj= I
==jjJ L
$numjjM N
)jjN O
?jjO P
.jjP Q
UserLinkjjQ Y
??jjZ \
$strjj] _
;jj_ `
}kk 
catchll 
(ll %
EndpointNotFoundExceptionll ,
)ll, -
{mm 

MessageBoxnn 
.nn 
Shownn 
(nn  
Langnn  $
.nn$ %"
ErrorServerUnavailablenn% ;
,nn; <
Langnn= A
.nnA B 
TitleConnectionErrornnB V
)nnV W
;nnW X
}oo 
catchpp 
(pp 
FaultExceptionpp !
expp" $
)pp$ %
{qq 

MessageBoxrr 
.rr 
Showrr 
(rr  
stringrr  &
.rr& '
Formatrr' -
(rr- .
Langrr. 2
.rr2 3
ErrorUnexpectedrr3 B
,rrB C
exrrD F
.rrF G
MessagerrG N
)rrN O
,rrO P
LangrrQ U
.rrU V

TitleErrorrrV `
)rr` a
;rra b
}ss 
}tt 	
privatevv 
staticvv 
boolvv !
CanExecuteSaveChangesvv 1
(vv1 2
objectvv2 8
	parametervv9 B
)vvB C
=>vvD F
truevvG K
;vvK L
privatexx 
voidxx 
ExecuteSaveChangesxx '
(xx' (
objectxx( .
	parameterxx/ 8
)xx8 9
{yy 	
stringzz 
	nameErrorzz 
=zz 
SignUpValidatorzz .
.zz. /
ValidateNamezz/ ;
(zz; <
Playerzz< B
.zzB C
namezzC G
)zzG H
;zzH I
if{{ 
({{ 
!{{ 
string{{ 
.{{ 
IsNullOrEmpty{{ %
({{% &
	nameError{{& /
){{/ 0
){{0 1
{{{2 3

MessageBox{{4 >
.{{> ?
Show{{? C
({{C D
	nameError{{D M
,{{M N
Lang{{O S
.{{S T
TitleValidation{{T c
){{c d
;{{d e
return{{f l
;{{l m
}{{n o
string}} 
lastNameError}}  
=}}! "
SignUpValidator}}# 2
.}}2 3
ValidateLastName}}3 C
(}}C D
Player}}D J
.}}J K
lastName}}K S
)}}S T
;}}T U
if~~ 
(~~ 
!~~ 
string~~ 
.~~ 
IsNullOrEmpty~~ %
(~~% &
lastNameError~~& 3
)~~3 4
)~~4 5
{~~6 7

MessageBox~~8 B
.~~B C
Show~~C G
(~~G H
lastNameError~~H U
,~~U V
Lang~~W [
.~~[ \
TitleValidation~~\ k
)~~k l
;~~l m
return~~n t
;~~t u
}~~v w
string
ÄÄ 
nicknameError
ÄÄ  
=
ÄÄ! "
SignUpValidator
ÄÄ# 2
.
ÄÄ2 3
ValidateNickname
ÄÄ3 C
(
ÄÄC D
Player
ÄÄD J
.
ÄÄJ K
nickname
ÄÄK S
)
ÄÄS T
;
ÄÄT U
if
ÅÅ 
(
ÅÅ 
!
ÅÅ 
string
ÅÅ 
.
ÅÅ 
IsNullOrEmpty
ÅÅ %
(
ÅÅ% &
nicknameError
ÅÅ& 3
)
ÅÅ3 4
)
ÅÅ4 5
{
ÅÅ6 7

MessageBox
ÅÅ8 B
.
ÅÅB C
Show
ÅÅC G
(
ÅÅG H
nicknameError
ÅÅH U
,
ÅÅU V
Lang
ÅÅW [
.
ÅÅ[ \
TitleValidation
ÅÅ\ k
)
ÅÅk l
;
ÅÅl m
return
ÅÅn t
;
ÅÅt u
}
ÅÅv w
var
ÉÉ 
passwordBox
ÉÉ 
=
ÉÉ 
	parameter
ÉÉ '
as
ÉÉ( *
System
ÉÉ+ 1
.
ÉÉ1 2
Windows
ÉÉ2 9
.
ÉÉ9 :
Controls
ÉÉ: B
.
ÉÉB C
PasswordBox
ÉÉC N
;
ÉÉN O
string
ÑÑ 
password
ÑÑ 
=
ÑÑ 
passwordBox
ÑÑ )
?
ÑÑ) *
.
ÑÑ* +
Password
ÑÑ+ 3
;
ÑÑ3 4
if
ÜÜ 
(
ÜÜ 
!
ÜÜ 
string
ÜÜ 
.
ÜÜ 
IsNullOrEmpty
ÜÜ %
(
ÜÜ% &
password
ÜÜ& .
)
ÜÜ. /
)
ÜÜ/ 0
{
áá 
string
àà 
passwordError
àà $
=
àà% &
SignUpValidator
àà' 6
.
àà6 7
ValidatePassword
àà7 G
(
ààG H
password
ààH P
)
ààP Q
;
ààQ R
if
ââ 
(
ââ 
!
ââ 
string
ââ 
.
ââ 
IsNullOrEmpty
ââ )
(
ââ) *
passwordError
ââ* 7
)
ââ7 8
)
ââ8 9
{
ää 

MessageBox
ãã 
.
ãã 
Show
ãã #
(
ãã# $
passwordError
ãã$ 1
,
ãã1 2
Lang
ãã3 7
.
ãã7 8
TitleValidation
ãã8 G
)
ããG H
;
ããH I
return
åå 
;
åå 
}
çç 
this
éé 
.
éé 
Player
éé 
.
éé 
password
éé $
=
éé% &
password
éé' /
;
éé/ 0
}
èè 
try
ëë 
{
íí 
var
ìì 
client
ìì 
=
ìì 
new
ìì  
UserProfileClient
ìì! 2
(
ìì2 3
)
ìì3 4
;
ìì4 5
bool
îî 
profileUpdated
îî #
=
îî$ %
client
îî& ,
.
îî, -
UpdatePlayer
îî- 9
(
îî9 :
this
îî: >
.
îî> ?
Player
îî? E
)
îîE F
;
îîF G
var
ññ 
socialsToUpdate
ññ #
=
ññ$ %
new
ññ& )
List
ññ* .
<
ññ. /
	SocialDto
ññ/ 8
>
ññ8 9
(
ññ9 :
)
ññ: ;
;
ññ; <
if
óó 
(
óó 
!
óó 
string
óó 
.
óó  
IsNullOrWhiteSpace
óó .
(
óó. /
InstagramLink
óó/ <
)
óó< =
)
óó= >
{
òò 
socialsToUpdate
ôô #
.
ôô# $
Add
ôô$ '
(
ôô' (
new
ôô( +
	SocialDto
ôô, 5
{
ôô6 7
IdSocialType
ôô8 D
=
ôôE F
$num
ôôG H
,
ôôH I
UserLink
ôôJ R
=
ôôS T
this
ôôU Y
.
ôôY Z
InstagramLink
ôôZ g
}
ôôh i
)
ôôi j
;
ôôj k
}
öö 
if
õõ 
(
õõ 
!
õõ 
string
õõ 
.
õõ  
IsNullOrWhiteSpace
õõ .
(
õõ. /
FacebookLink
õõ/ ;
)
õõ; <
)
õõ< =
{
úú 
socialsToUpdate
ùù #
.
ùù# $
Add
ùù$ '
(
ùù' (
new
ùù( +
	SocialDto
ùù, 5
{
ùù6 7
IdSocialType
ùù8 D
=
ùùE F
$num
ùùG H
,
ùùH I
UserLink
ùùJ R
=
ùùS T
this
ùùU Y
.
ùùY Z
FacebookLink
ùùZ f
}
ùùg h
)
ùùh i
;
ùùi j
}
ûû 
client
†† 
.
†† !
UpdatePlayerSocials
†† *
(
††* +
Player
††+ 1
.
††1 2
idPlayer
††2 :
,
††: ;
socialsToUpdate
††< K
.
††K L
ToArray
††L S
(
††S T
)
††T U
)
††U V
;
††V W
if
¢¢ 
(
¢¢ 
profileUpdated
¢¢ "
)
¢¢" #
{
££ 

MessageBox
§§ 
.
§§ 
Show
§§ #
(
§§# $
Lang
§§$ (
.
§§( )
InfoUpdateSuccess
§§) :
,
§§: ;
Lang
§§< @
.
§§@ A
TitleSuccess
§§A M
)
§§M N
;
§§N O
PlayerSession
•• !
.
••! "
CurrentPlayer
••" /
.
••/ 0
nickname
••0 8
=
••9 :
this
••; ?
.
••? @
Player
••@ F
.
••F G
nickname
••G O
;
••O P
ExecuteCancel
¶¶ !
(
¶¶! "
null
¶¶" &
)
¶¶& '
;
¶¶' (
}
ßß 
else
®® 
{
©© 

MessageBox
™™ 
.
™™ 
Show
™™ #
(
™™# $
Lang
™™$ (
.
™™( )
InfoUpdateFailed
™™) 9
,
™™9 :
Lang
™™; ?
.
™™? @

TitleError
™™@ J
)
™™J K
;
™™K L
}
´´ 
}
¨¨ 
catch
≠≠ 
(
≠≠ '
EndpointNotFoundException
≠≠ ,
)
≠≠, -
{
ÆÆ 

MessageBox
ØØ 
.
ØØ 
Show
ØØ 
(
ØØ  
Lang
ØØ  $
.
ØØ$ %$
ErrorServerUnavailable
ØØ% ;
,
ØØ; <
Lang
ØØ= A
.
ØØA B"
TitleConnectionError
ØØB V
)
ØØV W
;
ØØW X
}
∞∞ 
catch
±± 
(
±± 
FaultException
±± !
ex
±±" $
)
±±$ %
{
≤≤ 

MessageBox
≥≥ 
.
≥≥ 
Show
≥≥ 
(
≥≥  
string
≥≥  &
.
≥≥& '
Format
≥≥' -
(
≥≥- .
Lang
≥≥. 2
.
≥≥2 3
ErrorUnexpected
≥≥3 B
,
≥≥B C
ex
≥≥D F
.
≥≥F G
Message
≥≥G N
)
≥≥N O
,
≥≥O P
Lang
≥≥Q U
.
≥≥U V

TitleError
≥≥V `
)
≥≥` a
;
≥≥a b
}
¥¥ 
}
µµ 	
private
∑∑ 
static
∑∑ 
void
∑∑ 
ExecuteCancel
∑∑ )
(
∑∑) *
object
∑∑* 0
	parameter
∑∑1 :
)
∑∑: ;
{
∏∏ 	
ProfileMainFrame
ππ 
.
ππ 
	MainFrame
ππ &
.
ππ& '
Navigate
ππ' /
(
ππ/ 0
new
ππ0 3
UserProfilePage
ππ4 C
(
ππC D
)
ππD E
)
ππE F
;
ππF G
}
∫∫ 	
}
ªª 
}ºº ·
ÄC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\PresenceClientManager.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
{ 
public 

class !
PresenceClientManager &
{ 
private		 
static		 !
PresenceClientManager		 ,
instance		- 5
;		5 6
public

 
PresenceClient

 
Client

 $
{

% &
get

' *
;

* +
private

, 3
set

4 7
;

7 8
}

9 :
private !
PresenceClientManager %
(% &
)& '
{ 	
var 
context 
= 
new 
InstanceContext -
(- .
new. 1#
PresenceCallbackHandler2 I
(I J
)J K
)K L
;L M
Client 
= 
new 
PresenceClient '
(' (
context( /
)/ 0
;0 1
} 	
public 
static !
PresenceClientManager +
Instance, 4
{ 	
get 
{ 
if 
( 
instance 
== 
null  $
)$ %
{ 
instance 
= 
new "!
PresenceClientManager# 8
(8 9
)9 :
;: ;
} 
return 
instance 
;  
} 
} 	
} 
} Å
ÇC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\PresenceCallbackHandler.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
{ 
public 

class #
PresenceCallbackHandler (
:) *
IPresenceCallback+ <
{ 
public 
static 
event 
Action "
<" #
int# &
,& '
int( +
>+ ,
FriendStatusChanged- @
;@ A
public

 
void

 !
OnFriendStatusChanged

 )
(

) *
int

* -
friendId

. 6
,

6 7
int

8 ;
newStatusId

< G
)

G H
{ 	
FriendStatusChanged 
?  
.  !
Invoke! '
(' (
friendId( 0
,0 1
newStatusId2 =
)= >
;> ?
} 	
} 
} ∆
xC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\PlayerSession.cs
	namespace		 	
Conqui√°nCliente		
 
.		 
	ViewModel		 #
{

 
public 

static 
class 
PlayerSession %
{ 
public 
static 
PlayerLogin !
CurrentPlayer" /
{0 1
get2 5
;5 6
private7 >
set? B
;B C
}D E
public 
static 
bool 

IsLoggedIn %
=>& (
CurrentPlayer) 6
!=7 9
null: >
;> ?
public 
static 
void 
StartSession '
(' (
	PlayerDto( 1
player2 8
)8 9
{ 	
CurrentPlayer 
= 
player "
;" #
} 	
public 
static 
void 
UpdateSession (
(( )
ServiceUserProfile) ;
.; <
	PlayerDto< E
fullPlayerProfileF W
)W X
{ 	
if 
( 

IsLoggedIn 
&& 
CurrentPlayer +
.+ ,
nickname, 4
==5 7
fullPlayerProfile8 I
.I J
nicknameJ R
)R S
{ 
CurrentPlayer 
. 
name "
=# $
fullPlayerProfile% 6
.6 7
name7 ;
;; <
CurrentPlayer 
. 
lastName &
=' (
fullPlayerProfile) :
.: ;
lastName; C
;C D
CurrentPlayer 
. 
email #
=$ %
fullPlayerProfile& 7
.7 8
email8 =
;= >
CurrentPlayer 
. 
level #
=$ %
fullPlayerProfile& 7
.7 8
level8 =
;= >
CurrentPlayer 
. 
	pathPhoto '
=( )
fullPlayerProfile* ;
.; <
	pathPhoto< E
;E F
CurrentPlayer 
. 
currentPoints +
=, -
fullPlayerProfile. ?
.? @
currentPoints@ M
;M N
CurrentPlayer   
.   
nickname   &
=  ' (
fullPlayerProfile  ) :
.  : ;
nickname  ; C
;  C D
}!! 
}"" 	
public## 
static## 
void##  
UpdateProfilePicture## /
(##/ 0
string##0 6
newPhotoPath##7 C
)##C D
{$$ 	
if%% 
(%% 

IsLoggedIn%% 
)%% 
{&& 
CurrentPlayer'' 
.'' 
	pathPhoto'' '
=''( )
newPhotoPath''* 6
;''6 7
}(( 
})) 	
public,, 
static,, 
void,, 

EndSession,, %
(,,% &
),,& '
{-- 	
CurrentPlayer.. 
=.. 
null..  
;..  !
}// 	
}00 
}11 àX
ÖC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\MainMenu\MainMenuViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
MainMenu$ ,
{ 
public 

class 
MainMenuViewModel "
:# $
ViewModelBase% 2
{ 
public 
string 
Nickname 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
ProfileImagePath &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
ICommand 
ViewProfileCommand *
{+ ,
get- 0
;0 1
}2 3
public 
ICommand 
LogoutCommand %
{& '
get( +
;+ ,
}- .
public 
ICommand 
FriendsCommand &
{' (
get) ,
;, -
}. /
public 
ICommand 
PlayCommand #
{$ %
get& )
;) *
}+ ,
public 
ICommand !
ChangeLanguageCommand -
{. /
get0 3
;3 4
}5 6
public 
MainMenuViewModel  
(  !
)! "
{ 	
LoadPlayerData 
( 
) 
; 
ViewProfileCommand 
=  
new! $
RelayCommand% 1
(1 2
p2 3
=>4 6%
ExecuteViewProfileCommand7 P
(P Q
pQ R
)R S
)S T
;T U
LogoutCommand 
= 
new 
RelayCommand  ,
(, -
async- 2
(3 4
p4 5
)5 6
=>7 9
await: ? 
ExecuteLogoutCommand@ T
(T U
pU V
)V W
)W X
;X Y
FriendsCommand 
= 
new  
RelayCommand! -
(- .!
ExecuteFriendsCommand. C
)C D
;D E
PlayCommand 
= 
new 
RelayCommand *
(* +
ExecutePlay+ 6
)6 7
;7 8!
ChangeLanguageCommand   !
=  " #
new  $ '
RelayCommand  ( 4
(  4 5!
ExecuteChangeLanguage  5 J
)  J K
;  K L#
InvitationClientManager!! #
.!!# $
Connect!!$ +
(!!+ ,
PlayerSession!!, 9
.!!9 :
CurrentPlayer!!: G
.!!G H
idPlayer!!H P
)!!P Q
;!!Q R%
InvitationCallbackHandler"" %
.""% &&
OnGlobalInvitationReceived""& @
+=""A C
HandleInvitation""D T
;""T U!
PresenceClientManager## !
.##! "
Instance##" *
.##* +
Client##+ 1
.##1 2
	Subscribe##2 ;
(##; <
PlayerSession##< I
.##I J
CurrentPlayer##J W
.##W X
idPlayer##X `
)##` a
;##a b
}$$ 	
private&& 
void&& 
HandleInvitation&& %
(&&% &
string&&& ,
senderNickname&&- ;
,&&; <
string&&= C
roomCode&&D L
)&&L M
{'' 	
Application(( 
.(( 
Current(( 
.((  

Dispatcher((  *
.((* +
Invoke((+ 1
(((1 2
(((2 3
)((3 4
=>((5 7
{)) 
var** 
vm** 
=** 
new** '
InvitationReceivedViewModel** 8
(**8 9
senderNickname**9 G
,**G H
roomCode**I Q
)**Q R
;**R S
var++ 
window++ 
=++ 
new++  
View++! %
.++% &
Lobby++& +
.+++ ,$
InvitationReceivedWindow++, D
{++E F
DataContext++G R
=++S T
vm++U W
}++X Y
;++Y Z
window,, 
.,, 
Show,, 
(,, 
),, 
;,, 
}-- 
)-- 
;-- 
}.. 	
public00 
void00 
OnWindowClosing00 #
(00# $
)00$ %
{11 	%
InvitationCallbackHandler22 %
.22% &&
OnGlobalInvitationReceived22& @
-=22A C
HandleInvitation22D T
;22T U
}33 	
private55 
void55 
LoadPlayerData55 #
(55# $
)55$ %
{66 	
if77 
(77 
PlayerSession77 
.77 

IsLoggedIn77 (
)77( )
{88 
Nickname99 
=99 
PlayerSession99 (
.99( )
CurrentPlayer99) 6
.996 7
nickname997 ?
;99? @
ProfileImagePath::  
=::! "
PlayerSession::# 0
.::0 1
CurrentPlayer::1 >
.::> ?
	pathPhoto::? H
;::H I
};; 
}<< 	
private>> 
static>> 
void>> %
ExecuteViewProfileCommand>> 5
(>>5 6
object>>6 <
	parameter>>= F
)>>F G
{?? 	
ProfileMainFrame@@ 
userProfileView@@ ,
=@@- .
ProfileMainFrame@@/ ?
.@@? @
GetInstance@@@ K
(@@K L
)@@L M
;@@M N
userProfileViewAA 
.AA 
ShowAA  
(AA  !
)AA! "
;AA" #
(BB 
	parameterBB 
asBB 
WindowBB  
)BB  !
?BB! "
.BB" #
CloseBB# (
(BB( )
)BB) *
;BB* +
}CC 	
privateEE 
staticEE 
asyncEE 
TaskEE ! 
ExecuteLogoutCommandEE" 6
(EE6 7
objectEE7 =
	parameterEE> G
)EEG H
{FF 	
tryGG 
{HH 
varII 
loginClientII 
=II  !
newII" %
LoginClientII& 1
(II1 2
)II2 3
;II3 4
awaitJJ 
loginClientJJ !
.JJ! "
SignOutPlayerAsyncJJ" 4
(JJ4 5
PlayerSessionJJ5 B
.JJB C
CurrentPlayerJJC P
.JJP Q
idPlayerJJQ Y
)JJY Z
;JJZ [
awaitKK !
PresenceClientManagerKK +
.KK+ ,
InstanceKK, 4
.KK4 5
ClientKK5 ;
.KK; <
UnsubscribeAsyncKK< L
(KKL M
PlayerSessionKKM Z
.KKZ [
CurrentPlayerKK[ h
.KKh i
idPlayerKKi q
)KKq r
;KKr s
}LL 
catchMM 
(MM 
SystemMM 
.MM 
ServiceModelMM &
.MM& '%
EndpointNotFoundExceptionMM' @
)MM@ A
{NN 
}PP 
catchQQ 
(QQ 
	ExceptionQQ 
)QQ 
{RR 
}TT 
finallyUU 
{VV 
PlayerSessionWW 
.WW 

EndSessionWW (
(WW( )
)WW) *
;WW* +
varXX 
loginWindowXX 
=XX  !
newXX" %
LogInXX& +
(XX+ ,
)XX, -
;XX- .
loginWindowYY 
.YY 
ShowYY  
(YY  !
)YY! "
;YY" #
(ZZ 
	parameterZZ 
asZZ 
WindowZZ $
)ZZ$ %
?ZZ% &
.ZZ& '
CloseZZ' ,
(ZZ, -
)ZZ- .
;ZZ. /
}[[ 
}\\ 	
private^^ 
static^^ 
void^^ !
ExecuteFriendsCommand^^ 1
(^^1 2
object^^2 8
obj^^9 <
)^^< =
{__ 	
if`` 
(`` 
obj`` 
is`` 
Window`` 
mainMenuWindow`` ,
)``, -
{aa 
varbb 
friendListWindowbb $
=bb% &
newbb' *
Viewbb+ /
.bb/ 0

FriendListbb0 :
.bb: ;

FriendListbb; E
(bbE F
)bbF G
;bbG H
friendListWindowcc  
.cc  !
Showcc! %
(cc% &
)cc& '
;cc' (
mainMenuWindowdd 
.dd 
Closedd $
(dd$ %
)dd% &
;dd& '
}ee 
}ff 	
privatehh 
statichh 
voidhh 
ExecutePlayhh '
(hh' (
objecthh( .
	parameterhh/ 8
)hh8 9
{ii 	
ifjj 
(jj 
	parameterjj 
isjj 
Windowjj #
currentWindowjj$ 1
)jj1 2
{kk 
CreateOrJoinll 
createOrJoinViewll -
=ll. /
newll0 3
CreateOrJoinll4 @
(ll@ A
)llA B
;llB C
createOrJoinViewmm  
.mm  !
Ownermm! &
=mm' (
currentWindowmm) 6
;mm6 7
booloo 
?oo 
resultoo 
=oo 
createOrJoinViewoo /
.oo/ 0

ShowDialogoo0 :
(oo: ;
)oo; <
;oo< =
ifqq 
(qq 
resultqq 
==qq 
trueqq "
)qq" #
{rr 
varss 
createJoinViewModelss +
=ss, -
createOrJoinViewss. >
.ss> ?
DataContextss? J
asssK M!
CreateOrJoinViewModelssN c
;ssc d
stringtt 
newRoomCodett &
=tt' (
createJoinViewModeltt) <
.tt< =
CreatedRoomCodett= L
;ttL M
ifvv 
(vv 
!vv 
stringvv 
.vv  
IsNullOrEmptyvv  -
(vv- .
newRoomCodevv. 9
)vv9 :
)vv: ;
{ww 
	LobbyGamexx !
lobbyxx" '
=xx( )
newxx* -
	LobbyGamexx. 7
(xx7 8
newRoomCodexx8 C
)xxC D
;xxD E
lobbyyy 
.yy 
Showyy "
(yy" #
)yy# $
;yy$ %
currentWindowzz %
.zz% &
Closezz& +
(zz+ ,
)zz, -
;zz- .
}{{ 
}|| 
}}} 
}~~ 	
private
ÄÄ 
static
ÄÄ 
void
ÄÄ #
ExecuteChangeLanguage
ÄÄ 1
(
ÄÄ1 2
object
ÄÄ2 8
	parameter
ÄÄ9 B
)
ÄÄB C
{
ÅÅ 	
if
ÇÇ 
(
ÇÇ 
	parameter
ÇÇ 
is
ÇÇ 
Window
ÇÇ #
currentWindow
ÇÇ$ 1
)
ÇÇ1 2
{
ÉÉ 
var
ÑÑ 
selector
ÑÑ 
=
ÑÑ 
new
ÑÑ "
ChangeLanguage
ÑÑ# 1
(
ÑÑ1 2
)
ÑÑ2 3
;
ÑÑ3 4
selector
ÖÖ 
.
ÖÖ 
Owner
ÖÖ 
=
ÖÖ  
currentWindow
ÖÖ! .
;
ÖÖ. /
selector
ÜÜ 
.
ÜÜ 

ShowDialog
ÜÜ #
(
ÜÜ# $
)
ÜÜ$ %
;
ÜÜ% &
}
áá 
}
àà 	
}
ââ 
}ää ÉC
âC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\MainMenu\CreateOrJoinViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
MainMenu$ ,
{ 
public 

class !
CreateOrJoinViewModel &
:' (
ViewModelBase) 6
{ 
private 
string 
roomCode 
;  
public 
string 
RoomCode 
{ 	
get 
{ 
return 
roomCode !
;! "
}# $
set 
{ 
roomCode 
= 
value  
;  !
OnPropertyChanged !
(! "
nameof" (
(( )
RoomCode) 1
)1 2
)2 3
;3 4
} 
} 	
public 
string 
CreatedRoomCode %
{& '
get( +
;+ ,
private- 4
set5 8
;8 9
}: ;
public 
ICommand 
CreateRoomCommand )
{* +
get, /
;/ 0
}1 2
public 
ICommand 
JoinRoomCommand '
{( )
get* -
;- .
}/ 0
public   
ICommand   
CloseCommand   $
{  % &
get  ' *
;  * +
}  , -
public"" !
CreateOrJoinViewModel"" $
(""$ %
)""% &
{## 	
CreateRoomCommand$$ 
=$$ 
new$$  #
RelayCommand$$$ 0
($$0 1
async$$1 6
($$7 8
p$$8 9
)$$9 :
=>$$; =
await$$> C
ExecuteCreateRoom$$D U
($$U V
p$$V W
)$$W X
)$$X Y
;$$Y Z
JoinRoomCommand%% 
=%% 
new%% !
RelayCommand%%" .
(%%. /
async%%/ 4
(%%5 6
p%%6 7
)%%7 8
=>%%9 ;
await%%< A
ExecuteJoinRoom%%B Q
(%%Q R
p%%R S
)%%S T
)%%T U
;%%U V
CloseCommand&& 
=&& 
new&& 
RelayCommand&& +
(&&+ ,
ExecuteClose&&, 8
)&&8 9
;&&9 :
}'' 	
private)) 
async)) 
Task)) 
ExecuteCreateRoom)) ,
()), -
object))- 3
	parameter))4 =
)))= >
{** 	
if++ 
(++ 
	parameter++ 
is++ 
Window++ #
window++$ *
)++* +
{,, 
var-- 
client-- 
=-- 
new--  
LobbyClient--! ,
(--, -
new--- 0
InstanceContext--1 @
(--@ A
new--A D 
LobbyCallbackHandler--E Y
(--Y Z
)--Z [
)--[ \
)--\ ]
;--] ^
try.. 
{// 
CreatedRoomCode00 #
=00$ %
await00& +
client00, 2
.002 3
CreateLobbyAsync003 C
(00C D
PlayerSession00D Q
.00Q R
CurrentPlayer00R _
.00_ `
idPlayer00` h
)00h i
;00i j
if22 
(22 
!22 
string22 
.22  
IsNullOrEmpty22  -
(22- .
CreatedRoomCode22. =
)22= >
)22> ?
{33 
window44 
.44 
DialogResult44 +
=44, -
true44. 2
;442 3
window55 
.55 
Close55 $
(55$ %
)55% &
;55& '
}66 
else77 
{88 

MessageBox99 "
.99" #
Show99# '
(99' (
Lang99( ,
.99, -
ErrorLobbyCreation99- ?
,99? @
Lang99A E
.99E F

TitleError99F P
)99P Q
;99Q R
}:: 
};; 
catch<< 
(<< 
	Exception<<  
)<<  !
{== 

MessageBox>> 
.>> 
Show>> #
(>># $
Lang>>$ (
.>>( )#
ErrorConnectingToServer>>) @
,>>@ A
Lang>>B F
.>>F G

TitleError>>G Q
)>>Q R
;>>R S
}?? 
finally@@ 
{AA 
ifBB 
(BB 
clientBB 
.BB 
StateBB $
==BB% '
CommunicationStateBB( :
.BB: ;
OpenedBB; A
)BBA B
clientBBC I
.BBI J
CloseBBJ O
(BBO P
)BBP Q
;BBQ R
elseCC 
clientCC 
.CC  
AbortCC  %
(CC% &
)CC& '
;CC' (
}DD 
}EE 
}FF 	
privateHH 
asyncHH 
TaskHH 
ExecuteJoinRoomHH *
(HH* +
objectHH+ 1
	parameterHH2 ;
)HH; <
{II 	
ifJJ 
(JJ 
stringJJ 
.JJ 
IsNullOrWhiteSpaceJJ )
(JJ) *
RoomCodeJJ* 2
)JJ2 3
)JJ3 4
{KK 

MessageBoxLL 
.LL 
ShowLL 
(LL  
LangLL  $
.LL$ %
ErrorEmptyRoomCodeLL% 7
,LL7 8
LangLL9 =
.LL= >

TitleErrorLL> H
)LLH I
;LLI J
returnMM 
;MM 
}NN 
ifPP 
(PP 
	parameterPP 
isPP 
WindowPP #
windowPP$ *
)PP* +
{QQ 
varRR 
contextRR 
=RR 
newRR !
InstanceContextRR" 1
(RR1 2
newRR2 5 
LobbyCallbackHandlerRR6 J
(RRJ K
)RRK L
)RRL M
;RRM N
varSS 
clientSS 
=SS 
newSS  
LobbyClientSS! ,
(SS, -
contextSS- 4
)SS4 5
;SS5 6
tryTT 
{UU 
boolVV 
joinedSuccessfullyVV +
=VV, -
awaitVV. 3
clientVV4 :
.VV: ;!
JoinAndSubscribeAsyncVV; P
(VVP Q
RoomCodeVVQ Y
.VVY Z
ToUpperVVZ a
(VVa b
)VVb c
,VVc d
PlayerSessionVVe r
.VVr s
CurrentPlayer	VVs Ä
.
VVÄ Å
idPlayer
VVÅ â
)
VVâ ä
;
VVä ã
ifXX 
(XX 
joinedSuccessfullyXX *
)XX* +
{YY 
CreatedRoomCodeZZ '
=ZZ( )
RoomCodeZZ* 2
.ZZ2 3
ToUpperZZ3 :
(ZZ: ;
)ZZ; <
;ZZ< =
window[[ 
.[[ 
DialogResult[[ +
=[[, -
true[[. 2
;[[2 3
window\\ 
.\\ 
Close\\ $
(\\$ %
)\\% &
;\\& '
}]] 
else^^ 
{__ 

MessageBox`` "
.``" #
Show``# '
(``' (
Lang``( ,
.``, -
ErrorJoinLobby``- ;
,``; <
Lang``= A
.``A B

TitleError``B L
)``L M
;``M N
}aa 
}bb 
catchcc 
(cc %
EndpointNotFoundExceptioncc 0
)cc0 1
{dd 

MessageBoxee 
.ee 
Showee #
(ee# $
Langee$ (
.ee( )#
ErrorConnectingToServeree) @
,ee@ A
LangeeB F
.eeF G

TitleErroreeG Q
)eeQ R
;eeR S
}ff 
finallygg 
{hh 
ifii 
(ii 
clientii 
.ii 
Stateii $
==ii% '
CommunicationStateii( :
.ii: ;
Openedii; A
)iiA B
clientiiC I
.iiI J
CloseiiJ O
(iiO P
)iiP Q
;iiQ R
elsejj 
clientjj 
.jj  
Abortjj  %
(jj% &
)jj& '
;jj' (
}kk 
}ll 
}mm 	
privateoo 
staticoo 
voidoo 
ExecuteCloseoo (
(oo( )
objectoo) /
	parameteroo0 9
)oo9 :
{pp 	
ifqq 
(qq 
	parameterqq 
isqq 
Windowqq #
windowqq$ *
)qq* +
{rr 
windowss 
.ss 
Closess 
(ss 
)ss 
;ss 
}tt 
}uu 	
}vv 
}ww ∏
âC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Lobby\PlayerLobbyItemViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
Lobby$ )
{ 
public		 

class		 $
PlayerLobbyItemViewModel		 )
:		* +
ViewModelBase		, 9
{

 
private 
string 
displayName "
;" #
private 
string 
profileImagePath '
;' (
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
public 
string 
DisplayName !
{ 	
get 
{ 
return 
displayName $
;$ %
}& '
set 
{ 
displayName 
= 
value  %
;% &
OnPropertyChanged' 8
(8 9
nameof9 ?
(? @
DisplayName@ K
)K L
)L M
;M N
}O P
} 	
public 
string 
ProfileImagePath &
{ 	
get 
{ 
return 
profileImagePath )
;) *
}+ ,
set 
{ 
profileImagePath "
=# $
value% *
;* +
OnPropertyChanged, =
(= >
nameof> D
(D E
ProfileImagePathE U
)U V
)V W
;W X
}Y Z
} 	
} 
} ÁÊ
ÉC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Lobby\LobbyGameViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
Lobby$ )
{ 
public 

class 
LobbyGameViewModel #
:$ %
ViewModelBase& 3
{ 
private 
readonly 
string 
[  
]  !
	gameTypes" +
=, -
{. /
Lang0 4
.4 5
LobbyQuickGame5 C
,C D
LangE I
.I J
LobbyClassicGameJ Z
}[ \
;\ ]
private 
int 
currentGameIndex $
=% &
$num' (
;( )
private 
string 
selectedGameType '
;' (
private 
string 
playerCountText &
;& '
private 
string 
roomCode 
;  
private 
string 
currentMessage %
;% &
private 
bool 
isNavigatingAway %
=& '
false( -
;- .
private 
LobbyClient 
client "
;" #
private 
int 
idHost 
; 
private 
bool 

isHostBool 
;  
public 
bool 
IsHost 
{ 	
get 
{ 
return 

isHostBool #
;# $
}% &
set 
{ 

isHostBool 
= 
value "
;" #
OnPropertyChanged !
(! "
nameof" (
(( )
IsHost) /
)/ 0
)0 1
;1 2
}   
}!! 	
public##  
ObservableCollection## #
<### $$
PlayerLobbyItemViewModel##$ <
>##< =
Players##> E
{##F G
get##H K
;##K L
}##M N
public$$  
ObservableCollection$$ #
<$$# $
string$$$ *
>$$* +
ChatMessages$$, 8
{$$9 :
get$$; >
;$$> ?
}$$@ A
public&& 
string&& 
RoomCode&& 
{'' 	
get(( 
{(( 
return(( 
roomCode(( !
;((! "
}((# $
set)) 
{)) 
roomCode)) 
=)) 
value)) "
;))" #
OnPropertyChanged))$ 5
())5 6
nameof))6 <
())< =
RoomCode))= E
)))E F
)))F G
;))G H
}))I J
}** 	
public++ 
string++ 
SelectedGameType++ &
{,, 	
get-- 
{-- 
return-- 
selectedGameType-- )
;--) *
}--+ ,
set.. 
{.. 
selectedGameType.. "
=..# $
value..% *
;..* +
OnPropertyChanged.., =
(..= >
nameof..> D
(..D E
SelectedGameType..E U
)..U V
)..V W
;..W X
}..Y Z
}// 	
public00 
string00 
PlayerCountText00 %
{11 	
get22 
{22 
return22 
playerCountText22 (
;22( )
}22* +
set33 
{33 
playerCountText33 !
=33" #
value33$ )
;33) *
OnPropertyChanged33+ <
(33< =
nameof33= C
(33C D
PlayerCountText33D S
)33S T
)33T U
;33U V
}33W X
}44 	
public66 
string66 
CurrentMessage66 $
{77 	
get88 
{88 
return88 
currentMessage88 '
;88' (
}88) *
set99 
{99 
currentMessage99  
=99! "
value99# (
;99( )
OnPropertyChanged99* ;
(99; <
nameof99< B
(99B C
CurrentMessage99C Q
)99Q R
)99R S
;99S T
}99U V
}:: 	
public<< 
bool<< 
IsNavigatingAway<< $
{== 	
get>> 
{>> 
return>> 
isNavigatingAway>> )
;>>) *
}>>+ ,
set?? 
{@@ 
isNavigatingAwayAA  
=AA! "
valueAA# (
;AA( )
OnPropertyChangedBB !
(BB! "
nameofBB" (
(BB( )
IsNavigatingAwayBB) 9
)BB9 :
)BB: ;
;BB; <
}CC 
}DD 	
publicFF 
ICommandFF 
NextGameTypeCommandFF +
{FF, -
getFF. 1
;FF1 2
}FF3 4
publicGG 
ICommandGG #
PreviousGameTypeCommandGG /
{GG0 1
getGG2 5
;GG5 6
}GG7 8
publicHH 
ICommandHH 
GoBackCommandHH %
{HH& '
getHH( +
;HH+ ,
}HH- .
publicII 
ICommandII 
SendMessageCommandII *
{II+ ,
getII- 0
;II0 1
}II2 3
publicJJ 
ICommandJJ $
ShowInviteFriendsCommandJJ 0
{JJ1 2
getJJ3 6
;JJ6 7
}JJ8 9
publicKK 
ICommandKK &
ShutdownApplicationCommandKK 2
{KK3 4
getKK5 8
;KK8 9
}KK: ;
publicMM 
LobbyGameViewModelMM !
(MM! "
stringMM" (
receivedRoomCodeMM) 9
)MM9 :
{NN 	
PlayersOO 
=OO 
newOO  
ObservableCollectionOO .
<OO. /$
PlayerLobbyItemViewModelOO/ G
>OOG H
(OOH I
)OOI J
;OOJ K
ChatMessagesPP 
=PP 
newPP  
ObservableCollectionPP 3
<PP3 4
stringPP4 :
>PP: ;
(PP; <
)PP< =
;PP= >
thisQQ 
.QQ 
RoomCodeQQ 
=QQ 
receivedRoomCodeQQ ,
;QQ, -
SelectedGameTypeRR 
=RR 
	gameTypesRR (
[RR( )
currentGameIndexRR) 9
]RR9 :
;RR: ;
NextGameTypeCommandTT 
=TT  !
newTT" %
RelayCommandTT& 2
(TT2 3
ExecuteNextGameTypeTT3 F
)TTF G
;TTG H#
PreviousGameTypeCommandUU #
=UU$ %
newUU& )
RelayCommandUU* 6
(UU6 7#
ExecutePreviousGameTypeUU7 N
)UUN O
;UUO P
GoBackCommandVV 
=VV 
newVV 
RelayCommandVV  ,
(VV, -
ExecuteGoBackVV- :
)VV: ;
;VV; <
SendMessageCommandWW 
=WW  
newWW! $
RelayCommandWW% 1
(WW1 2
ExecuteSendMessageWW2 D
,WWD E!
CanExecuteSendMessageWWF [
)WW[ \
;WW\ ]$
ShowInviteFriendsCommandXX $
=XX% &
newXX' *
RelayCommandXX+ 7
(XX7 8$
ExecuteShowInviteFriendsXX8 P
,XXP Q'
CanExecuteShowInviteFriendsXXR m
)XXm n
;XXn o&
ShutdownApplicationCommandYY &
=YY' (
newYY) ,
RelayCommandYY- 9
(YY9 :&
ExecuteShutdownApplicationYY: T
)YYT U
;YYU V
_[[ 
=[[ %
InitializeConnectionAsync[[ '
([[' (
)[[( )
;[[) *
}\\ 	
private^^ 
async^^ 
Task^^ %
InitializeConnectionAsync^^ 4
(^^4 5
)^^5 6
{__ 	
try`` 
{aa 
varbb 
callbackHandlerbb #
=bb$ %
newbb& ) 
LobbyCallbackHandlerbb* >
(bb> ?
)bb? @
;bb@ A
callbackHandlerdd 
.dd  
OnPlayerJoineddd  .
+=dd/ 1
HandlePlayerJoineddd2 D
;ddD E
callbackHandleree 
.ee  
OnPlayerLeftee  ,
+=ee- /
HandlePlayerLeftee0 @
;ee@ A
callbackHandlerff 
.ff  

OnHostLeftff  *
+=ff+ -
HandleHostLeftff. <
;ff< =
callbackHandlergg 
.gg  
OnMessageReceivedgg  1
+=gg2 4!
HandleMessageReceivedgg5 J
;ggJ K
varii 
contextii 
=ii 
newii !
InstanceContextii" 1
(ii1 2
callbackHandlerii2 A
)iiA B
;iiB C
clientjj 
=jj 
newjj 
LobbyClientjj (
(jj( )
contextjj) 0
)jj0 1
;jj1 2
varll 

lobbyStatell 
=ll  
awaitll! &
clientll' -
.ll- .
GetLobbyStateAsyncll. @
(ll@ A
thisllA E
.llE F
RoomCodellF N
)llN O
;llO P
ifmm 
(mm 
stringmm 
.mm 
IsNullOrEmptymm (
(mm( )

lobbyStatemm) 3
.mm3 4
RoomCodemm4 <
)mm< =
)mm= >
{nn 
HandleHostLeftoo "
(oo" #
)oo# $
;oo$ %
returnpp 
;pp 
}qq 
idHostss 
=ss 

lobbyStatess #
.ss# $
idHostPlayerss$ 0
;ss0 1
thistt 
.tt 
IsHosttt 
=tt 
(tt 
PlayerSessiontt ,
.tt, -
CurrentPlayertt- :
.tt: ;
idPlayertt; C
==ttD F
idHostttG M
)ttM N
;ttN O
UpdatePlayerListuu  
(uu  !

lobbyStateuu! +
.uu+ ,
Playersuu, 3
)uu3 4
;uu4 5

UpdateChatvv 
(vv 

lobbyStatevv %
.vv% &
ChatMessagesvv& 2
)vv2 3
;vv3 4
awaitxx 
clientxx 
.xx !
JoinAndSubscribeAsyncxx 2
(xx2 3
thisxx3 7
.xx7 8
RoomCodexx8 @
,xx@ A
PlayerSessionxxB O
.xxO P
CurrentPlayerxxP ]
.xx] ^
idPlayerxx^ f
)xxf g
;xxg h
}yy 
catchzz 
(zz 
	Exceptionzz 
)zz 
{{{ 

MessageBox|| 
.|| 
Show|| 
(||  
Lang||  $
.||$ %#
ErrorConnectingToServer||% <
,||< =
Lang||> B
.||B C

TitleError||C M
,||M N
MessageBoxButton||O _
.||_ `
OK||` b
,||b c
MessageBoxImage||d s
.||s t
Error||t y
)||y z
;||z {
NavigateToMainMenu}} "
(}}" #
)}}# $
;}}$ %
}~~ 
} 	
private
ÅÅ 
void
ÅÅ (
ExecuteShutdownApplication
ÅÅ /
(
ÅÅ/ 0
object
ÅÅ0 6
obj
ÅÅ7 :
)
ÅÅ: ;
{
ÇÇ 	
if
ÉÉ 
(
ÉÉ 
IsNavigatingAway
ÉÉ  
)
ÉÉ  !
return
ÉÉ" (
;
ÉÉ( )
IsNavigatingAway
ÑÑ 
=
ÑÑ 
true
ÑÑ #
;
ÑÑ# $#
CloseClientConnection
ÜÜ !
(
ÜÜ! "
notifyServer
ÜÜ" .
:
ÜÜ. /
true
ÜÜ0 4
)
ÜÜ4 5
;
ÜÜ5 6
Application
áá 
.
áá 
Current
áá 
.
áá  
Shutdown
áá  (
(
áá( )
)
áá) *
;
áá* +
}
àà 	
private
ää 
void
ää  
HandlePlayerJoined
ää '
(
ää' (
	PlayerDto
ää( 1
	newPlayer
ää2 ;
)
ää; <
{
ãã 	
Application
åå 
.
åå 
Current
åå 
.
åå  

Dispatcher
åå  *
.
åå* +
Invoke
åå+ 1
(
åå1 2
(
åå2 3
)
åå3 4
=>
åå5 7
{
çç 
if
éé 
(
éé 
!
éé 
Players
éé 
.
éé 
Any
éé  
(
éé  !
p
éé! "
=>
éé# %
p
éé& '
.
éé' (
Id
éé( *
==
éé+ -
	newPlayer
éé. 7
.
éé7 8
idPlayer
éé8 @
)
éé@ A
)
ééA B
{
èè 
Players
êê 
.
êê 
Add
êê 
(
êê  #
CreatePlayerViewModel
êê  5
(
êê5 6
	newPlayer
êê6 ?
)
êê? @
)
êê@ A
;
êêA B
UpdatePlayerCount
ëë %
(
ëë% &
)
ëë& '
;
ëë' (
}
íí 
}
ìì 
)
ìì 
;
ìì 
}
îî 	
private
ññ 
void
ññ 
HandlePlayerLeft
ññ %
(
ññ% &
int
ññ& )
idPlayer
ññ* 2
)
ññ2 3
{
óó 	
Application
òò 
.
òò 
Current
òò 
.
òò  

Dispatcher
òò  *
.
òò* +
Invoke
òò+ 1
(
òò1 2
(
òò2 3
)
òò3 4
=>
òò5 7
{
ôô 
var
öö 
playerToRemove
öö "
=
öö# $
Players
öö% ,
.
öö, -
FirstOrDefault
öö- ;
(
öö; <
p
öö< =
=>
öö> @
p
ööA B
.
ööB C
Id
ööC E
==
ööF H
idPlayer
ööI Q
)
ööQ R
;
ööR S
if
õõ 
(
õõ 
playerToRemove
õõ "
!=
õõ# %
null
õõ& *
)
õõ* +
{
úú 
Players
ùù 
.
ùù 
Remove
ùù "
(
ùù" #
playerToRemove
ùù# 1
)
ùù1 2
;
ùù2 3
UpdatePlayerCount
ûû %
(
ûû% &
)
ûû& '
;
ûû' (
}
üü 
}
†† 
)
†† 
;
†† 
}
°° 	
private
££ 
void
££ 
HandleHostLeft
££ #
(
££# $
)
££$ %
{
§§ 	
if
•• 
(
•• 
IsNavigatingAway
••  
)
••  !
return
••" (
;
••( )
IsNavigatingAway
¶¶ 
=
¶¶ 
true
¶¶ #
;
¶¶# $
Application
®® 
.
®® 
Current
®® 
.
®®  

Dispatcher
®®  *
.
®®* +
Invoke
®®+ 1
(
®®1 2
(
®®2 3
)
®®3 4
=>
®®5 7
{
©© 

MessageBox
™™ 
.
™™ 
Show
™™ 
(
™™  
Lang
™™  $
.
™™$ %
InfoHostLeft
™™% 1
,
™™1 2
Lang
™™3 7
.
™™7 8
Lobby
™™8 =
,
™™= >
MessageBoxButton
™™? O
.
™™O P
OK
™™P R
,
™™R S
MessageBoxImage
™™T c
.
™™c d
Information
™™d o
)
™™o p
;
™™p q#
CloseClientConnection
¨¨ %
(
¨¨% &
notifyServer
¨¨& 2
:
¨¨2 3
false
¨¨4 9
)
¨¨9 :
;
¨¨: ; 
NavigateToMainMenu
≠≠ "
(
≠≠" #
)
≠≠# $
;
≠≠$ %
}
ÆÆ 
)
ÆÆ 
;
ÆÆ 
}
ØØ 	
private
±± 
void
±± #
HandleMessageReceived
±± *
(
±±* +

MessageDto
±±+ 5
message
±±6 =
)
±±= >
{
≤≤ 	
Application
≥≥ 
.
≥≥ 
Current
≥≥ 
.
≥≥  

Dispatcher
≥≥  *
.
≥≥* +
Invoke
≥≥+ 1
(
≥≥1 2
(
≥≥2 3
)
≥≥3 4
=>
≥≥5 7
{
¥¥ 
ChatMessages
µµ 
.
µµ 
Add
µµ  
(
µµ  !
$"
µµ! #
{
µµ# $
message
µµ$ +
.
µµ+ ,
Nickname
µµ, 4
}
µµ4 5
$str
µµ5 7
{
µµ7 8
message
µµ8 ?
.
µµ? @
Message
µµ@ G
}
µµG H
"
µµH I
)
µµI J
;
µµJ K
}
∂∂ 
)
∂∂ 
;
∂∂ 
}
∑∑ 	
private
ππ 
void
ππ 
UpdatePlayerList
ππ %
(
ππ% &
	PlayerDto
ππ& /
[
ππ/ 0
]
ππ0 1
players
ππ2 9
)
ππ9 :
{
∫∫ 	
Players
ªª 
.
ªª 
Clear
ªª 
(
ªª 
)
ªª 
;
ªª 
foreach
ºº 
(
ºº 
var
ºº 
	playerDto
ºº "
in
ºº# %
players
ºº& -
)
ºº- .
{
ΩΩ 
Players
ææ 
.
ææ 
Add
ææ 
(
ææ #
CreatePlayerViewModel
ææ 1
(
ææ1 2
	playerDto
ææ2 ;
)
ææ; <
)
ææ< =
;
ææ= >
}
øø 
UpdatePlayerCount
¿¿ 
(
¿¿ 
)
¿¿ 
;
¿¿  
}
¡¡ 	
private
√√ 
void
√√ 

UpdateChat
√√ 
(
√√  

MessageDto
√√  *
[
√√* +
]
√√+ ,
messages
√√- 5
)
√√5 6
{
ƒƒ 	
ChatMessages
≈≈ 
.
≈≈ 
Clear
≈≈ 
(
≈≈ 
)
≈≈  
;
≈≈  !
if
∆∆ 
(
∆∆ 
messages
∆∆ 
!=
∆∆ 
null
∆∆  
)
∆∆  !
{
«« 
foreach
»» 
(
»» 
var
»» 
message
»» $
in
»»% '
messages
»»( 0
)
»»0 1
{
…… 
ChatMessages
    
.
    !
Add
  ! $
(
  $ %
$"
  % '
{
  ' (
message
  ( /
.
  / 0
Nickname
  0 8
}
  8 9
$str
  9 ;
{
  ; <
message
  < C
.
  C D
Message
  D K
}
  K L
"
  L M
)
  M N
;
  N O
}
ÀÀ 
}
ÃÃ 
}
ÕÕ 	
private
œœ &
PlayerLobbyItemViewModel
œœ (#
CreatePlayerViewModel
œœ) >
(
œœ> ?
	PlayerDto
œœ? H
	playerDto
œœI R
)
œœR S
{
–– 	
var
—— 

playerItem
—— 
=
—— 
new
——  &
PlayerLobbyItemViewModel
——! 9
{
““ 
Id
”” 
=
”” 
	playerDto
”” 
.
”” 
idPlayer
”” '
,
””' (
ProfileImagePath
‘‘  
=
‘‘! "
	playerDto
‘‘# ,
.
‘‘, -
	pathPhoto
‘‘- 6
,
‘‘6 7
DisplayName
’’ 
=
’’ 
	playerDto
’’ '
.
’’' (
nickname
’’( 0
}
÷÷ 
;
÷÷ 
if
ÿÿ 
(
ÿÿ 
	playerDto
ÿÿ 
.
ÿÿ 
idPlayer
ÿÿ "
==
ÿÿ# %
this
ÿÿ& *
.
ÿÿ* +
idHost
ÿÿ+ 1
)
ÿÿ1 2
{
ŸŸ 

playerItem
⁄⁄ 
.
⁄⁄ 
DisplayName
⁄⁄ &
=
⁄⁄' (
$"
⁄⁄) +
{
⁄⁄+ ,
Lang
⁄⁄, 0
.
⁄⁄0 1
LobbyHostPrefix
⁄⁄1 @
}
⁄⁄@ A
$str
⁄⁄A B
{
⁄⁄B C
	playerDto
⁄⁄C L
.
⁄⁄L M
nickname
⁄⁄M U
}
⁄⁄U V
"
⁄⁄V W
;
⁄⁄W X
}
€€ 
return
‹‹ 

playerItem
‹‹ 
;
‹‹ 
}
›› 	
private
ﬂﬂ 
void
ﬂﬂ 
UpdatePlayerCount
ﬂﬂ &
(
ﬂﬂ& '
)
ﬂﬂ' (
{
‡‡ 	
int
·· 

maxPlayers
·· 
=
·· 
$num
·· 
;
·· 
PlayerCountText
‚‚ 
=
‚‚ 
$"
‚‚  
{
‚‚  !
Players
‚‚! (
.
‚‚( )
Count
‚‚) .
}
‚‚. /
$str
‚‚/ 0
{
‚‚0 1

maxPlayers
‚‚1 ;
}
‚‚; <
"
‚‚< =
;
‚‚= >
}
„„ 	
private
ÂÂ 
bool
ÂÂ #
CanExecuteSendMessage
ÂÂ *
(
ÂÂ* +
object
ÂÂ+ 1
obj
ÂÂ2 5
)
ÂÂ5 6
{
ÊÊ 	
return
ÁÁ 
!
ÁÁ 
string
ÁÁ 
.
ÁÁ  
IsNullOrWhiteSpace
ÁÁ -
(
ÁÁ- .
CurrentMessage
ÁÁ. <
)
ÁÁ< =
;
ÁÁ= >
}
ËË 	
private
ÍÍ 
void
ÍÍ  
ExecuteSendMessage
ÍÍ '
(
ÍÍ' (
object
ÍÍ( .
obj
ÍÍ/ 2
)
ÍÍ2 3
{
ÎÎ 	
var
ÏÏ 

messageDto
ÏÏ 
=
ÏÏ 
new
ÏÏ  

MessageDto
ÏÏ! +
{
ÌÌ 
Nickname
ÓÓ 
=
ÓÓ 
PlayerSession
ÓÓ (
.
ÓÓ( )
CurrentPlayer
ÓÓ) 6
.
ÓÓ6 7
nickname
ÓÓ7 ?
,
ÓÓ? @
Message
ÔÔ 
=
ÔÔ 
this
ÔÔ 
.
ÔÔ 
CurrentMessage
ÔÔ -
,
ÔÔ- .
	Timestamp
 
=
 
DateTime
 $
.
$ %
UtcNow
% +
}
ÒÒ 
;
ÒÒ 
Task
ÛÛ 
.
ÛÛ 
Run
ÛÛ 
(
ÛÛ 
async
ÛÛ 
(
ÛÛ 
)
ÛÛ 
=>
ÛÛ  
{
ÙÙ 
try
ıı 
{
ˆˆ 
await
˜˜ 
client
˜˜  
.
˜˜  !
SendMessageAsync
˜˜! 1
(
˜˜1 2
this
˜˜2 6
.
˜˜6 7
RoomCode
˜˜7 ?
,
˜˜? @

messageDto
˜˜A K
)
˜˜K L
;
˜˜L M
}
¯¯ 
catch
˘˘ 
(
˘˘ 
	Exception
˘˘  
)
˘˘  !
{
˙˙ 

MessageBox
˚˚ 
.
˚˚ 
Show
˚˚ #
(
˚˚# $
Lang
˚˚$ (
.
˚˚( )$
ErrorSendMessageFailed
˚˚) ?
,
˚˚? @
Lang
˚˚A E
.
˚˚E F

TitleError
˚˚F P
,
˚˚P Q
MessageBoxButton
˚˚R b
.
˚˚b c
OK
˚˚c e
,
˚˚e f
MessageBoxImage
˚˚g v
.
˚˚v w
Error
˚˚w |
)
˚˚| }
;
˚˚} ~
}
¸¸ 
}
˝˝ 
)
˝˝ 
;
˝˝ 
CurrentMessage
ˇˇ 
=
ˇˇ 
string
ˇˇ #
.
ˇˇ# $
Empty
ˇˇ$ )
;
ˇˇ) *
}
ÄÄ 	
private
ÇÇ 
void
ÇÇ 
ExecuteGoBack
ÇÇ "
(
ÇÇ" #
object
ÇÇ# )
	parameter
ÇÇ* 3
)
ÇÇ3 4
{
ÉÉ 	
if
ÑÑ 
(
ÑÑ 
isNavigatingAway
ÑÑ  
)
ÑÑ  !
return
ÑÑ" (
;
ÑÑ( )
isNavigatingAway
ÖÖ 
=
ÖÖ 
true
ÖÖ #
;
ÖÖ# $#
CloseClientConnection
áá !
(
áá! "
notifyServer
áá" .
:
áá. /
true
áá0 4
)
áá4 5
;
áá5 6 
NavigateToMainMenu
àà 
(
àà 
	parameter
àà (
as
àà) +
Window
àà, 2
)
àà2 3
;
àà3 4
}
ââ 	
private
ãã 
void
ãã #
CloseClientConnection
ãã *
(
ãã* +
bool
ãã+ /
notifyServer
ãã0 <
)
ãã< =
{
åå 	
if
çç 
(
çç 
client
çç 
==
çç 
null
çç 
)
çç 
return
çç  &
;
çç& '
try
èè 
{
êê 
if
ëë 
(
ëë 
client
ëë 
.
ëë 
State
ëë  
==
ëë! # 
CommunicationState
ëë$ 6
.
ëë6 7
Opened
ëë7 =
)
ëë= >
{
íí 
if
ìì 
(
ìì 
notifyServer
ìì $
)
ìì$ %
{
îî 
client
ïï 
.
ïï !
LeaveAndUnsubscribe
ïï 2
(
ïï2 3
this
ïï3 7
.
ïï7 8
RoomCode
ïï8 @
,
ïï@ A
PlayerSession
ïïB O
.
ïïO P
CurrentPlayer
ïïP ]
.
ïï] ^
idPlayer
ïï^ f
)
ïïf g
;
ïïg h
}
ññ 
client
óó 
.
óó 
Close
óó  
(
óó  !
)
óó! "
;
óó" #
}
òò 
else
ôô 
{
öö 
client
õõ 
.
õõ 
Abort
õõ  
(
õõ  !
)
õõ! "
;
õõ" #
}
úú 
}
ùù 
catch
ûû 
(
ûû 
	Exception
ûû 
)
ûû 
{
üü 
client
†† 
.
†† 
Abort
†† 
(
†† 
)
†† 
;
†† 
}
°° 
finally
¢¢ 
{
££ 
client
§§ 
=
§§ 
null
§§ 
;
§§ 
}
•• 
}
¶¶ 	
private
®® 
void
®®  
NavigateToMainMenu
®® '
(
®®' (
Window
®®( .
currentWindow
®®/ <
=
®®= >
null
®®? C
)
®®C D
{
©© 	
var
™™ 
mainMenu
™™ 
=
™™ 
new
™™ 
View
™™ #
.
™™# $
MainMenu
™™$ ,
.
™™, -
MainMenu
™™- 5
(
™™5 6
)
™™6 7
;
™™7 8
mainMenu
´´ 
.
´´ 
Show
´´ 
(
´´ 
)
´´ 
;
´´ 
var
≠≠ 
windowToClose
≠≠ 
=
≠≠ 
currentWindow
≠≠  -
;
≠≠- .
if
ÆÆ 
(
ÆÆ 
windowToClose
ÆÆ 
==
ÆÆ  
null
ÆÆ! %
)
ÆÆ% &
{
ØØ 
foreach
∞∞ 
(
∞∞ 
Window
∞∞ 
window
∞∞  &
in
∞∞' )
Application
∞∞* 5
.
∞∞5 6
Current
∞∞6 =
.
∞∞= >
Windows
∞∞> E
.
∞∞E F
OfType
∞∞F L
<
∞∞L M
Window
∞∞M S
>
∞∞S T
(
∞∞T U
)
∞∞U V
.
∞∞V W
ToList
∞∞W ]
(
∞∞] ^
)
∞∞^ _
)
∞∞_ `
{
±± 
if
≤≤ 
(
≤≤ 
window
≤≤ 
.
≤≤ 
DataContext
≤≤ *
==
≤≤+ -
this
≤≤. 2
)
≤≤2 3
{
≥≥ 
windowToClose
¥¥ %
=
¥¥& '
window
¥¥( .
;
¥¥. /
break
µµ 
;
µµ 
}
∂∂ 
}
∑∑ 
}
∏∏ 
if
∫∫ 
(
∫∫ 
windowToClose
∫∫ 
!=
∫∫  
null
∫∫! %
)
∫∫% &
{
ªª 
try
ºº 
{
ΩΩ 
windowToClose
ææ !
.
ææ! "
Close
ææ" '
(
ææ' (
)
ææ( )
;
ææ) *
}
øø 
catch
¿¿ 
(
¿¿ '
InvalidOperationException
¿¿ 0
)
¿¿0 1
{
¿¿2 3
}
¿¿4 5
}
¡¡ 
}
¬¬ 	
private
ƒƒ 
bool
ƒƒ )
CanExecuteShowInviteFriends
ƒƒ 0
(
ƒƒ0 1
object
ƒƒ1 7
obj
ƒƒ8 ;
)
ƒƒ; <
{
≈≈ 	
return
∆∆ 
IsHost
∆∆ 
;
∆∆ 
}
«« 	
private
…… 
void
…… &
ExecuteShowInviteFriends
…… -
(
……- .
object
……. 4
obj
……5 8
)
……8 9
{
   	
var
ÀÀ 
vm
ÀÀ 
=
ÀÀ 
new
ÀÀ $
InviteFriendsViewModel
ÀÀ /
(
ÀÀ/ 0
this
ÀÀ0 4
.
ÀÀ4 5
RoomCode
ÀÀ5 =
)
ÀÀ= >
;
ÀÀ> ?
var
ÃÃ 
window
ÃÃ 
=
ÃÃ 
new
ÃÃ !
InviteFriendsWindow
ÃÃ 0
{
ÕÕ 
DataContext
ŒŒ 
=
ŒŒ 
vm
ŒŒ  
,
ŒŒ  !
Owner
œœ 
=
œœ 
Application
œœ #
.
œœ# $
Current
œœ$ +
.
œœ+ ,
Windows
œœ, 3
.
œœ3 4
OfType
œœ4 :
<
œœ: ;
Window
œœ; A
>
œœA B
(
œœB C
)
œœC D
.
œœD E
FirstOrDefault
œœE S
(
œœS T
w
œœT U
=>
œœV X
w
œœY Z
.
œœZ [
DataContext
œœ[ f
==
œœg i
this
œœj n
)
œœn o
}
–– 
;
–– 
window
—— 
.
—— 

ShowDialog
—— 
(
—— 
)
—— 
;
——  
}
““ 	
private
‘‘ 
void
‘‘ !
ExecuteNextGameType
‘‘ (
(
‘‘( )
object
‘‘) /
obj
‘‘0 3
)
‘‘3 4
{
’’ 	
currentGameIndex
÷÷ 
=
÷÷ 
(
÷÷  
currentGameIndex
÷÷  0
+
÷÷1 2
$num
÷÷3 4
)
÷÷4 5
%
÷÷6 7
	gameTypes
÷÷8 A
.
÷÷A B
Length
÷÷B H
;
÷÷H I
SelectedGameType
◊◊ 
=
◊◊ 
	gameTypes
◊◊ (
[
◊◊( )
currentGameIndex
◊◊) 9
]
◊◊9 :
;
◊◊: ;
}
ÿÿ 	
private
⁄⁄ 
void
⁄⁄ %
ExecutePreviousGameType
⁄⁄ ,
(
⁄⁄, -
object
⁄⁄- 3
obj
⁄⁄4 7
)
⁄⁄7 8
{
€€ 	
currentGameIndex
‹‹ 
=
‹‹ 
(
‹‹  
currentGameIndex
‹‹  0
-
‹‹1 2
$num
‹‹3 4
+
‹‹5 6
	gameTypes
‹‹7 @
.
‹‹@ A
Length
‹‹A G
)
‹‹G H
%
‹‹I J
	gameTypes
‹‹K T
.
‹‹T U
Length
‹‹U [
;
‹‹[ \
SelectedGameType
›› 
=
›› 
	gameTypes
›› (
[
››( )
currentGameIndex
››) 9
]
››9 :
;
››: ;
}
ﬁﬁ 	
}
ﬂﬂ 
}‡‡ õ
ÖC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Lobby\LobbyCallbackHandler.cs
	namespace		 	
Conqui√°nCliente		
 
.		 
	ViewModel		 #
.		# $
Lobby		$ )
{

 
[ 
CallbackBehavior 
( %
UseSynchronizationContext /
=0 1
false2 7
)7 8
]8 9
public 

class  
LobbyCallbackHandler %
:& '
ILobbyCallback( 6
{ 
public 
event 
Action 
< 
	PlayerDto %
>% &
OnPlayerJoined' 5
;5 6
public 
event 
Action 
< 
int 
>  
OnPlayerLeft! -
;- .
public 
event 
Action 

OnHostLeft &
;& '
public 
event 
Action 
< 

MessageDto &
>& '
OnMessageReceived( 9
;9 :
public 
void 
HostLeft 
( 
) 
{ 	

OnHostLeft 
? 
. 
Invoke 
( 
)  
;  !
} 	
public 
void 
MessageReceived #
(# $

MessageDto$ .
message/ 6
)6 7
{ 	
OnMessageReceived 
? 
. 
Invoke %
(% &
message& -
)- .
;. /
} 	
public 
void 
PlayerJoined  
(  !
	PlayerDto! *
	newPlayer+ 4
)4 5
{ 	
OnPlayerJoined 
? 
. 
Invoke "
(" #
	newPlayer# ,
), -
;- .
}   	
public"" 
void"" 

PlayerLeft"" 
("" 
int"" "
idPlayer""# +
)""+ ,
{## 	
OnPlayerLeft$$ 
?$$ 
.$$ 
Invoke$$  
($$  !
idPlayer$$! )
)$$) *
;$$* +
}%% 	
}&& 
}'' †I
áC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Lobby\InviteFriendsViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
Lobby$ )
{ 
public 

class "
InviteFriendsViewModel '
:( )
ViewModelBase* 7
{ 
private 
readonly 
string 
roomCode  (
;( )
public  
ObservableCollection #
<# $%
FriendInviteItemViewModel$ =
>= >
FriendsList? J
{K L
getM P
;P Q
}R S
public 
ICommand 
InviteFriendCommand +
{, -
get. 1
;1 2
}3 4
public "
InviteFriendsViewModel %
(% &
string& ,
roomCode- 5
)5 6
{ 	
this 
. 
roomCode 
= 
roomCode $
;$ %
FriendsList 
= 
new  
ObservableCollection 2
<2 3%
FriendInviteItemViewModel3 L
>L M
(M N
)N O
;O P
InviteFriendCommand 
=  !
new" %
RelayCommand& 2
(2 3
async3 8
(9 :
param: ?
)? @
=>A C
awaitD I
ExecuteInviteFriendJ ]
(] ^
param^ c
)c d
)d e
;e f#
PresenceCallbackHandler #
.# $
FriendStatusChanged$ 7
+=8 :!
OnFriendStatusChanged; P
;P Q
_ 
= 
LoadFriends 
( 
) 
; 
} 	
private 
void !
OnFriendStatusChanged *
(* +
int+ .
friendId/ 7
,7 8
int9 <
newStatusId= H
)H I
{ 	
var 
friendVM 
= 
FriendsList &
.& '
FirstOrDefault' 5
(5 6
f6 7
=>8 :
f; <
.< =
IdPlayer= E
==F H
friendIdI Q
)Q R
;R S
if 
( 
friendVM 
!= 
null  
)  !
{   
bool!! 
isOnline!! 
=!! 
(!!  !
newStatusId!!! ,
==!!- /
$num!!0 1
)!!1 2
;!!2 3
friendVM"" 
."" 
IsOnline"" !
=""" #
isOnline""$ ,
;"", -
friendVM## 
.## 

StatusText## #
=##$ %
isOnline##& .
?##/ 0
Lang##1 5
.##5 6
StatusOnline##6 B
:##C D
Lang##E I
.##I J
StatusOffline##J W
;##W X
}$$ 
}%% 	
private&& 
async&& 
Task&& 
LoadFriends&& &
(&&& '
)&&' (
{'' 	
try(( 
{)) 
using** 
(** 
var** 
client** !
=**" #
new**$ '
FriendListClient**( 8
(**8 9
)**9 :
)**: ;
{++ 
var,, 
friends,, 
=,,  !
await,," '
client,,( .
.,,. /
GetFriendsAsync,,/ >
(,,> ?
PlayerSession,,? L
.,,L M
CurrentPlayer,,M Z
.,,Z [
idPlayer,,[ c
),,c d
;,,d e
FriendsList-- 
.--  
Clear--  %
(--% &
)--& '
;--' (
foreach.. 
(.. 
var..  
friend..! '
in..( *
friends..+ 2
...2 3
OrderByDescending..3 D
(..D E
f..E F
=>..G I
f..J K
...K L
idStatus..L T
)..T U
)..U V
{// 
FriendsList00 #
.00# $
Add00$ '
(00' (
new00( +%
FriendInviteItemViewModel00, E
(00E F
friend00F L
)00L M
)00M N
;00N O
}11 
}22 
}33 
catch44 
(44 
	Exception44 
ex44 
)44  
{55 
System66 
.66 
Windows66 
.66 

MessageBox66 )
.66) *
Show66* .
(66. /
Lang66/ 3
.663 4$
LobbyErrorLoadingFriends664 L
+66M N
$"66O Q
$str66Q S
{66S T
ex66T V
.66V W
Message66W ^
}66^ _
"66_ `
)66` a
;66a b
}77 
}88 	
private:: 
async:: 
Task:: 
ExecuteInviteFriend:: .
(::. /
object::/ 5
	parameter::6 ?
)::? @
{;; 	
if<< 
(<< 
	parameter<< 
is<< %
FriendInviteItemViewModel<< 6
friendVM<<7 ?
)<<? @
{== 
bool>> 
success>> 
=>> 
await>> $#
InvitationClientManager>>% <
.>>< =
SendInvitation>>= K
(>>K L
PlayerSession?? !
.??! "
CurrentPlayer??" /
.??/ 0
idPlayer??0 8
,??8 9
PlayerSession@@ !
.@@! "
CurrentPlayer@@" /
.@@/ 0
nickname@@0 8
,@@8 9
friendVMAA 
.AA 
IdPlayerAA %
,AA% &
thisBB 
.BB 
roomCodeBB !
)CC 
;CC 
ifEE 
(EE 
successEE 
)EE 
{FF 
friendVMGG 
.GG 

StatusTextGG '
=GG( )
LangGG* .
.GG. /
LobbyInvitationSentGG/ B
;GGB C
friendVMHH 
.HH 
IsOnlineHH %
=HH& '
falseHH( -
;HH- .
}II 
elseJJ 
{KK 
SystemLL 
.LL 
WindowsLL "
.LL" #

MessageBoxLL# -
.LL- .
ShowLL. 2
(LL2 3
LangLL3 7
.LL7 8&
LobbyErrorInvitationFailedLL8 R
)LLR S
;LLS T
}MM 
}NN 
}OO 	
}PP 
publicRR 

classRR %
FriendInviteItemViewModelRR *
:RR+ ,
ViewModelBaseRR- :
{SS 
privateTT 
readonlyTT 
	PlayerDtoTT "
friendTT# )
;TT) *
privateUU 
stringUU 

statusTextUU !
;UU! "
privateVV 
boolVV 
isOnlineVV 
;VV 
publicWW 
stringWW 
LevelWW 
=>WW 
friendWW %
.WW% &
levelWW& +
;WW+ ,
publicXX %
FriendInviteItemViewModelXX (
(XX( )
	PlayerDtoXX) 2
friendXX3 9
)XX9 :
{YY 	
thisZZ 
.ZZ 
friendZZ 
=ZZ 
friendZZ  
;ZZ  !
this[[ 
.[[ 
IsOnline[[ 
=[[ 
friend[[ "
.[[" #
idStatus[[# +
==[[, .
$num[[/ 0
;[[0 1
this\\ 
.\\ 

StatusText\\ 
=\\ 
this\\ "
.\\" #
IsOnline\\# +
?\\, -
Lang\\. 2
.\\2 3
StatusOnline\\3 ?
:\\@ A
Lang\\B F
.\\F G
StatusOffline\\G T
;\\T U
}]] 	
public__ 
int__ 
IdPlayer__ 
=>__ 
friend__ %
.__% &
idPlayer__& .
;__. /
public`` 
string`` 
Nickname`` 
=>`` !
friend``" (
.``( )
nickname``) 1
;``1 2
publicaa 
stringaa 
ProfileImagePathaa &
=>aa' )
friendaa* 0
.aa0 1
	pathPhotoaa1 :
;aa: ;
publicbb 
boolbb 
IsOnlinebb 
{cc 	
getdd 
=>dd 
isOnlinedd 
;dd 
setee 
{ee 
isOnlineee 
=ee 
valueee "
;ee" #
OnPropertyChangedee$ 5
(ee5 6
nameofee6 <
(ee< =
IsOnlineee= E
)eeE F
)eeF G
;eeG H
}eeI J
}ff 	
publichh 
stringhh 

StatusTexthh  
{ii 	
getjj 
=>jj 

statusTextjj 
;jj 
setkk 
{kk 

statusTextkk 
=kk 
valuekk $
;kk$ %
OnPropertyChangedkk& 7
(kk7 8
nameofkk8 >
(kk> ?

StatusTextkk? I
)kkI J
)kkJ K
;kkK L
}kkM N
}ll 	
publicnn 
Brushnn 
StatusColornn  
=>nn! #
IsOnlinenn$ ,
?nn- .
Brushesnn/ 6
.nn6 7
Greennn7 <
:nn= >
Brushesnn? F
.nnF G
GraynnG K
;nnK L
}oo 
}pp ˜2
åC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Lobby\InvitationReceivedViewModel.cs
	namespace

 	
Conqui√°nCliente


 
.

 
	ViewModel

 #
.

# $
Lobby

$ )
{ 
public 

class '
InvitationReceivedViewModel ,
:- .
ViewModelBase/ <
{ 
private 
readonly 
string 
roomCode  (
;( )
public 
string 
InvitationText $
{% &
get' *
;* +
}, -
public 
ICommand 
AcceptCommand %
{& '
get( +
;+ ,
}- .
public 
ICommand 
RejectCommand %
{& '
get( +
;+ ,
}- .
public '
InvitationReceivedViewModel *
(* +
string+ 1
senderNickname2 @
,@ A
stringB H
roomCodeI Q
)Q R
{ 	
this 
. 
roomCode 
= 
roomCode $
;$ %
this 
. 
InvitationText 
=  !
$"" $
{$ %
senderNickname% 3
}3 4
$str4 5
{5 6
Lang6 :
.: ;
LobbyInvitedYou; J
}J K
"K L
;L M
AcceptCommand 
= 
new 
RelayCommand  ,
(, -
ExecuteAccept- :
): ;
;; <
RejectCommand 
= 
new 
RelayCommand  ,
(, -
ExecuteReject- :
): ;
;; <
} 	
private 
async 
void 
ExecuteAccept (
(( )
object) /
	parameter0 9
)9 :
{ 	
var 
window 
= 
	parameter "
as# %
Window& ,
;, -
LobbyDto 

lobbyState 
=  !
null" &
;& '
try   
{!! 
using"" 
("" 
var"" 
lobbyClient"" &
=""' (
new"") ,
LobbyClient""- 8
(""8 9
new""9 <
InstanceContext""= L
(""L M
new""M P 
LobbyCallbackHandler""Q e
(""e f
)""f g
)""g h
)""h i
)""i j
{## 

lobbyState$$ 
=$$  
await$$! &
lobbyClient$$' 2
.$$2 3
GetLobbyStateAsync$$3 E
($$E F
this$$F J
.$$J K
roomCode$$K S
)$$S T
;$$T U
}%% 
}&& 
catch'' 
('' 
	Exception'' 
)'' 
{(( 

MessageBox)) 
.)) 
Show)) 
())  
Lang))  $
.))$ %#
ErrorConnectingToServer))% <
,))< =
Lang))> B
.))B C

TitleError))C M
)))M N
;))N O
window** 
?** 
.** 
Close** 
(** 
)** 
;**  
return++ 
;++ 
},, 
if.. 
(.. 

lobbyState.. 
==.. 
null.. "
).." #
{// 

MessageBox00 
.00 
Show00 
(00  
Lang00  $
.00$ %
InfoHostLeft00% 1
,001 2
Lang003 7
.007 8
Lobby008 =
)00= >
;00> ?
window11 
?11 
.11 
Close11 
(11 
)11 
;11  
return22 
;22 
}33 
if55 
(55 

lobbyState55 
.55 
Players55 "
.55" #
Length55# )
>=55* ,
$num55- .
)55. /
{66 

MessageBox77 
.77 
Show77 
(77  
Lang77  $
.77$ %
	LobbyFull77% .
,77. /
Lang770 4
.774 5
Lobby775 :
)77: ;
;77; <
window88 
?88 
.88 
Close88 
(88 
)88 
;88  
return99 
;99 
}:: 
try== 
{>> 
var?? 
	lobbyGame?? 
=?? 
new??  #
	LobbyGame??$ -
(??- .
this??. 2
.??2 3
roomCode??3 ;
)??; <
;??< =
	lobbyGame@@ 
.@@ 
Show@@ 
(@@ 
)@@  
;@@  !
varAA 
windowsToCloseAA "
=AA# $
ApplicationAA% 0
.AA0 1
CurrentAA1 8
.AA8 9
WindowsAA9 @
.AA@ A
OfTypeAAA G
<AAG H
WindowAAH N
>AAN O
(AAO P
)AAP Q
.BB 
WhereBB "
(BB" #
wBB# $
=>BB% '
wBB( )
!=BB* ,
	lobbyGameBB- 6
)BB6 7
.CC 
ToListCC #
(CC# $
)CC$ %
;CC% &
foreachDD 
(DD 
WindowDD 

openWindowDD  *
inDD+ -
windowsToCloseDD. <
)DD< =
{EE 

openWindowFF 
.FF 
CloseFF $
(FF$ %
)FF% &
;FF& '
}GG 
}HH 
catchII 
(II 
	ExceptionII 
exII 
)II  
{JJ 

MessageBoxKK 
.KK 
ShowKK 
(KK  
$"KK  "
{KK" #
LangKK# '
.KK' (#
ErrorConnectingToServerKK( ?
}KK? @
$strKK@ B
{KKB C
exKKC E
.KKE F
MessageKKF M
}KKM N
"KKN O
,KKO P
LangKKQ U
.KKU V

TitleErrorKKV `
)KK` a
;KKa b
}LL 
}MM 	
privateOO 
staticOO 
voidOO 
ExecuteRejectOO )
(OO) *
objectOO* 0
	parameterOO1 :
)OO: ;
{PP 	
(QQ 
	parameterQQ 
asQQ 
WindowQQ  
)QQ  !
?QQ! "
.QQ" #
CloseQQ# (
(QQ( )
)QQ) *
;QQ* +
}RR 	
}SS 
}TT –	
ÑC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\InvitationCallbackHandler.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
{ 
[ 
CallbackBehavior 
( %
UseSynchronizationContext /
=0 1
false2 7
)7 8
]8 9
public 

class %
InvitationCallbackHandler *
:+ ,&
IInvitationServiceCallback- G
{		 
public

 
static

 
event

 
Action

 "
<

" #
string

# )
,

) *
string

+ 1
>

1 2&
OnGlobalInvitationReceived

3 M
;

M N
public 
void  
OnInvitationReceived (
(( )
string) /
senderNickname0 >
,> ?
string@ F
roomCodeG O
)O P
{ 	&
OnGlobalInvitationReceived &
?& '
.' (
Invoke( .
(. /
senderNickname/ =
,= >
roomCode? G
)G H
;H I
} 	
} 
} «!
ÇC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\InvitationClientManager.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
{		 
public

 

static

 
class

 #
InvitationClientManager

 /
{ 
private 
static #
InvitationServiceClient .
client/ 5
;5 6
public 
static 
void 
Connect "
(" #
int# &
idPlayer' /
)/ 0
{ 	
if 
( 
client 
!= 
null 
&& !
client" (
.( )
State) .
==/ 1
CommunicationState2 D
.D E
OpenedE K
)K L
{ 
return 
; 
} 
try 
{ 
var 
callbackHandler #
=$ %
new& )%
InvitationCallbackHandler* C
(C D
)D E
;E F
var 
context 
= 
new !
InstanceContext" 1
(1 2
callbackHandler2 A
)A B
;B C
client 
= 
new #
InvitationServiceClient 4
(4 5
context5 <
)< =
;= >
client 
. 
	Subscribe  
(  !
idPlayer! )
)) *
;* +
} 
catch 
( 
	Exception 
ex 
)  
{ 

MessageBox 
. 
Show 
(  
ex  "
." #
Message# *
,* +
Lang, 0
.0 1
ErrorUnexpected1 @
)A B
;B C
} 
}   	
public"" 
static"" 
void"" 

Disconnect"" %
(""% &
int""& )
idPlayer""* 2
)""2 3
{## 	
if$$ 
($$ 
client$$ 
!=$$ 
null$$ 
&&$$ !
client$$" (
.$$( )
State$$) .
==$$/ 1
CommunicationState$$2 D
.$$D E
Opened$$E K
)$$K L
{%% 
try&& 
{'' 
client(( 
.(( 
Unsubscribe(( &
(((& '
idPlayer((' /
)((/ 0
;((0 1
client)) 
.)) 
Close))  
())  !
)))! "
;))" #
}** 
catch++ 
(++ 
	Exception++  
)++  !
{,, 
client-- 
.-- 
Abort--  
(--  !
)--! "
;--" #
}.. 
}// 
client00 
=00 
null00 
;00 
}11 	
public33 
static33 
async33 
Task33  
<33  !
bool33! %
>33% &
SendInvitation33' 5
(335 6
int336 9
idSender33: B
,33B C
string33D J
senderNickname33K Y
,33Y Z
int33[ ^

idReceiver33_ i
,33i j
string33k q
roomCode33r z
)33z {
{44 	
if55 
(55 
client55 
==55 
null55 
||55 !
client55" (
.55( )
State55) .
!=55/ 1
CommunicationState552 D
.55D E
Opened55E K
)55K L
{66 
Connect77 
(77 
idSender77  
)77  !
;77! "
}88 
try:: 
{;; 
return<< 
await<< 
client<< #
.<<# $
SendInvitationAsync<<$ 7
(<<7 8
idSender<<8 @
,<<@ A
senderNickname<<B P
,<<P Q

idReceiver<<R \
,<<\ ]
roomCode<<^ f
)<<f g
;<<g h
}== 
catch>> 
(>> 
	Exception>> 
)>> 
{?? 
return@@ 
false@@ 
;@@ 
}AA 
}BB 	
}CC 
}DD ˝≠
âC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Authentication\SignUpViewModel.cs
	namespace

 	
Conqui√°nCliente


 
.

 
	ViewModel

 #
.

# $
Authentication

$ 2
{ 
public 

class 
SignUpViewModel  
:! "
ViewModelBase# 0
{ 
private 
string 
email 
; 
private 
string 
name 
; 
private 
string 
lastName 
;  
private 
string 
nickname 
;  
private 
string #
enteredVerificationCode .
;. /
private 
readonly 
	PlayerDto "
playerInProgress# 3
;3 4
public 
string 
Email 
{ 	
get 
{ 
return 
email 
; 
}  !
set 
{ 
email 
= 
value 
;  
OnPropertyChanged! 2
(2 3
)3 4
;4 5
}6 7
} 	
public 
string 
Name 
{ 	
get 
{ 
return 
name 
; 
}  
set 
{ 
name 
= 
value 
; 
OnPropertyChanged  1
(1 2
)2 3
;3 4
}5 6
} 	
public 
string 
LastName 
{   	
get!! 
{!! 
return!! 
lastName!! !
;!!! "
}!!# $
set"" 
{"" 
lastName"" 
="" 
value"" "
;""" #
OnPropertyChanged""$ 5
(""5 6
)""6 7
;""7 8
}""9 :
}## 	
public$$ 
string$$ 
Nickname$$ 
{%% 	
get&& 
{&& 
return&& 
nickname&& !
;&&! "
}&&# $
set'' 
{'' 
nickname'' 
='' 
value'' "
;''" #
OnPropertyChanged''$ 5
(''5 6
)''6 7
;''7 8
}''9 :
}(( 	
public)) 
string)) #
EnteredVerificationCode)) -
{** 	
get++ 
{++ 
return++ #
enteredVerificationCode++ 0
;++0 1
}++2 3
set,, 
{,, #
enteredVerificationCode,, )
=,,* +
value,,, 1
;,,1 2
OnPropertyChanged,,3 D
(,,D E
),,E F
;,,F G
},,H I
}-- 	
public// 
ICommand// '
SendVerificationCodeCommand// 3
{//4 5
get//6 9
;//9 :
}//; <
public00 
ICommand00 
VerifyCodeCommand00 )
{00* +
get00, /
;00/ 0
}001 2
public11 
ICommand11 !
RegisterPlayerCommand11 -
{11. /
get110 3
;113 4
}115 6
public22 
ICommand22 "
NavigateToLoginCommand22 .
{22/ 0
get221 4
;224 5
}226 7
public33 
ICommand33 #
NavigateToSignUpCommand33 /
{330 1
get332 5
;335 6
}337 8
public55 
SignUpViewModel55 
(55 
)55  
{66 	
playerInProgress77 
=77 
new77 "
	PlayerDto77# ,
(77, -
)77- .
;77. /'
SendVerificationCodeCommand99 '
=99( )
new99* -
RelayCommand99. :
(99: ;'
ExecuteSendVerificationCode99; V
,99V W*
CanExecuteSendVerificationCode99X v
)99v w
;99w x"
NavigateToLoginCommand:: "
=::# $
new::% (
RelayCommand::) 5
(::5 6"
ExecuteNavigateToLogin::6 L
)::L M
;::M N
VerifyCodeCommand<< 
=<< 
new<<  #
RelayCommand<<$ 0
(<<0 1
ExecuteVerifyCode<<1 B
,<<B C 
CanExecuteVerifyCode<<D X
)<<X Y
;<<Y Z#
NavigateToSignUpCommand== #
===$ %
new==& )
RelayCommand==* 6
(==6 7#
ExecuteNavigateToSignUp==7 N
)==N O
;==O P!
RegisterPlayerCommand?? !
=??" #
new??$ '
RelayCommand??( 4
(??4 5!
ExecuteRegisterPlayer??5 J
,??J K$
CanExecuteRegisterPlayer??L d
)??d e
;??e f
}@@ 	
privateBB 
staticBB 
boolBB *
CanExecuteSendVerificationCodeBB :
(BB: ;
objectBB; A
	parameterBBB K
)BBK L
=>BBM O
trueBBP T
;BBT U
privateCC 
staticCC 
boolCC  
CanExecuteVerifyCodeCC 0
(CC0 1
objectCC1 7
	parameterCC8 A
)CCA B
=>CCC E
trueCCF J
;CCJ K
privateDD 
staticDD 
boolDD $
CanExecuteRegisterPlayerDD 4
(DD4 5
objectDD5 ;
	parameterDD< E
)DDE F
=>DDG I
trueDDJ N
;DDN O
privateFF 
asyncFF 
voidFF '
ExecuteSendVerificationCodeFF 6
(FF6 7
objectFF7 =
	parameterFF> G
)FFG H
{GG 	
varHH 
passwordBoxHH 
=HH 
	parameterHH '
asHH( *
PasswordBoxHH+ 6
;HH6 7
stringII 
passwordII 
=II 
passwordBoxII )
?II) *
.II* +
PasswordII+ 3
;II3 4
varKK 
windowKK 
=KK 
WindowKK 
.KK  
	GetWindowKK  )
(KK) *
passwordBoxKK* 5
)KK5 6
;KK6 7
varLL 
confirmPasswordBoxLL "
=LL# $
windowLL% +
?LL+ ,
.LL, -
FindNameLL- 5
(LL5 6
$strLL6 I
)LLI J
asLLK M
PasswordBoxLLN Y
;LLY Z
stringMM 
confirmPasswordMM "
=MM# $
confirmPasswordBoxMM% 7
?MM7 8
.MM8 9
PasswordMM9 A
;MMA B
stringOO 

emailErrorOO 
=OO 
SignUpValidatorOO  /
.OO/ 0
ValidateEmailOO0 =
(OO= >
EmailOO> C
)OOC D
;OOD E
ifPP 
(PP 
!PP 
stringPP 
.PP 
IsNullOrEmptyPP %
(PP% &

emailErrorPP& 0
)PP0 1
)PP1 2
{QQ 

MessageBoxRR 
.RR 
ShowRR 
(RR  

emailErrorRR  *
,RR* +
LangRR, 0
.RR0 1
TitleValidationRR1 @
)RR@ A
;RRA B
returnSS 
;SS 
}TT 
EmailVV 
=VV 
EmailVV 
.VV 
TrimVV 
(VV 
)VV  
;VV  !
stringXX 
passwordErrorXX  
=XX! "
SignUpValidatorXX# 2
.XX2 3
ValidatePasswordXX3 C
(XXC D
passwordXXD L
)XXL M
;XXM N
ifYY 
(YY 
!YY 
stringYY 
.YY 
IsNullOrEmptyYY %
(YY% &
passwordErrorYY& 3
)YY3 4
)YY4 5
{ZZ 

MessageBox[[ 
.[[ 
Show[[ 
([[  
passwordError[[  -
,[[- .
Lang[[/ 3
.[[3 4
TitleValidation[[4 C
)[[C D
;[[D E
return\\ 
;\\ 
}]] 
string^^  
confirmPasswordError^^ '
=^^( )
SignUpValidator^^* 9
.^^9 :#
ValidateConfirmPassword^^: Q
(^^Q R
password^^R Z
,^^Z [
confirmPassword^^\ k
)^^k l
;^^l m
if__ 
(__ 
!__ 
string__ 
.__ 
IsNullOrEmpty__ %
(__% & 
confirmPasswordError__& :
)__: ;
)__; <
{`` 

MessageBoxaa 
.aa 
Showaa 
(aa   
confirmPasswordErroraa  4
,aa4 5
Langaa6 :
.aa: ;
TitleValidationaa; J
)aaJ K
;aaK L
returnbb 
;bb 
}cc 
tryee 
{ff 
vargg 
clientgg 
=gg 
newgg  
SignUpClientgg! -
(gg- .
)gg. /
;gg/ 0
stringhh 
serverResponsehh %
=hh& '
awaithh( -
clienthh. 4
.hh4 5%
SendVerificationCodeAsynchh5 N
(hhN O
EmailhhO T
)hhT U
;hhU V
ifjj 
(jj 
serverResponsejj "
==jj# %
$strjj& :
)jj: ;
{kk 

MessageBoxll 
.ll 
Showll #
(ll# $
Langll$ (
.ll( )
ErrorEmailExistsll) 9
,ll9 :
Langll; ?
.ll? @

TitleErrorll@ J
)llJ K
;llK L
}mm 
elsenn 
ifnn 
(nn 
!nn 
stringnn  
.nn  !
IsNullOrEmptynn! .
(nn. /
serverResponsenn/ =
)nn= >
)nn> ?
{oo 
playerInProgresspp $
.pp$ %
emailpp% *
=pp+ ,
Emailpp- 2
;pp2 3
playerInProgressqq $
.qq$ %
passwordqq% -
=qq. /
passwordqq0 8
;qq8 9
varss 
verificationWindowss *
=ss+ ,
newss- 0
VerificationCodess1 A
(ssA B
)ssB C
;ssC D
verificationWindowtt &
.tt& '
DataContexttt' 2
=tt3 4
thistt5 9
;tt9 :
verificationWindowuu &
.uu& '
Showuu' +
(uu+ ,
)uu, -
;uu- .
Windowvv 
.vv 
	GetWindowvv $
(vv$ %
passwordBoxvv% 0
)vv0 1
?vv1 2
.vv2 3
Closevv3 8
(vv8 9
)vv9 :
;vv: ;
}ww 
elsexx 
{yy 

MessageBoxzz 
.zz 
Showzz #
(zz# $
Langzz$ (
.zz( )"
ErrorVerificationEmailzz) ?
,zz? @
LangzzA E
.zzE F

TitleErrorzzF P
)zzP Q
;zzQ R
}{{ 
}|| 
catch}} 
(}} %
EndpointNotFoundException}} ,
)}}, -
{~~ 

MessageBox 
. 
Show 
(  
Lang  $
.$ %"
ErrorServerUnavailable% ;
,; <
Lang= A
.A B 
TitleConnectionErrorB V
)V W
;W X
}
ÄÄ 
catch
ÅÅ 
(
ÅÅ 
System
ÅÅ 
.
ÅÅ 
	Exception
ÅÅ #
ex
ÅÅ$ &
)
ÅÅ& '
{
ÇÇ 

MessageBox
ÉÉ 
.
ÉÉ 
Show
ÉÉ 
(
ÉÉ  
string
ÉÉ  &
.
ÉÉ& '
Format
ÉÉ' -
(
ÉÉ- .
Lang
ÉÉ. 2
.
ÉÉ2 3
ErrorGeneric
ÉÉ3 ?
,
ÉÉ? @
ex
ÉÉA C
.
ÉÉC D
Message
ÉÉD K
)
ÉÉK L
,
ÉÉL M
Lang
ÉÉN R
.
ÉÉR S

TitleError
ÉÉS ]
)
ÉÉ] ^
;
ÉÉ^ _
}
ÑÑ 
}
ÖÖ 	
private
áá 
async
áá 
void
áá 
ExecuteVerifyCode
áá ,
(
áá, -
object
áá- 3
	parameter
áá4 =
)
áá= >
{
àà 	
if
ââ 
(
ââ 
string
ââ 
.
ââ 
IsNullOrEmpty
ââ $
(
ââ$ %%
EnteredVerificationCode
ââ% <
)
ââ< =
)
ââ= >
{
ää 

MessageBox
ãã 
.
ãã 
Show
ãã 
(
ãã  
string
ãã  &
.
ãã& '
Format
ãã' -
(
ãã- .
Lang
ãã. 2
.
ãã2 3(
ErrorVerificationCodeEmpty
ãã3 M
)
ããM N
)
ããN O
;
ããO P
return
åå 
;
åå 
}
çç 
try
èè 
{
êê 
var
ëë 
client
ëë 
=
ëë 
new
ëë  
SignUpClient
ëë! -
(
ëë- .
)
ëë. /
;
ëë/ 0
bool
íí 
isCodeValid
íí  
=
íí! "
await
íí# (
client
íí) /
.
íí/ 0
VerifyCodeAsync
íí0 ?
(
íí? @
playerInProgress
íí@ P
.
ííP Q
email
ííQ V
,
ííV W%
EnteredVerificationCode
ííX o
)
íío p
;
ííp q
if
îî 
(
îî 
isCodeValid
îî 
)
îî  
{
ïï 
var
ññ 
signUpDataWindow
ññ (
=
ññ) *
new
ññ+ .

SignUpData
ññ/ 9
(
ññ9 :
)
ññ: ;
;
ññ; <
signUpDataWindow
óó $
.
óó$ %
DataContext
óó% 0
=
óó1 2
this
óó3 7
;
óó7 8
signUpDataWindow
òò $
.
òò$ %
Show
òò% )
(
òò) *
)
òò* +
;
òò+ ,
(
ôô 
	parameter
ôô 
as
ôô !
Window
ôô" (
)
ôô( )
?
ôô) *
.
ôô* +
Close
ôô+ 0
(
ôô0 1
)
ôô1 2
;
ôô2 3
}
öö 
else
õõ 
{
úú 

MessageBox
ùù 
.
ùù 
Show
ùù #
(
ùù# $
Lang
ùù$ (
.
ùù( ),
ErrorVerificationCodeIncorrect
ùù) G
,
ùùG H
Lang
ùùI M
.
ùùM N

TitleError
ùùN X
)
ùùX Y
;
ùùY Z
}
ûû 
}
üü 
catch
†† 
(
†† 
System
†† 
.
†† 
	Exception
†† #
ex
††$ &
)
††& '
{
°° 

MessageBox
¢¢ 
.
¢¢ 
Show
¢¢ 
(
¢¢  
string
¢¢  &
.
¢¢& '
Format
¢¢' -
(
¢¢- .
Lang
¢¢. 2
.
¢¢2 3
ErrorGeneric
¢¢3 ?
,
¢¢? @
ex
¢¢A C
.
¢¢C D
Message
¢¢D K
)
¢¢K L
,
¢¢L M
Lang
¢¢N R
.
¢¢R S

TitleError
¢¢S ]
)
¢¢] ^
;
¢¢^ _
}
££ 
}
§§ 	
private
¶¶ 
async
¶¶ 
void
¶¶ #
ExecuteRegisterPlayer
¶¶ 0
(
¶¶0 1
object
¶¶1 7
	parameter
¶¶8 A
)
¶¶A B
{
ßß 	
string
®® 
	nameError
®® 
=
®® 
SignUpValidator
®® .
.
®®. /
ValidateName
®®/ ;
(
®®; <
Name
®®< @
)
®®@ A
;
®®A B
if
©© 
(
©© 
!
©© 
string
©© 
.
©© 
IsNullOrEmpty
©© %
(
©©% &
	nameError
©©& /
)
©©/ 0
)
©©0 1
{
™™ 

MessageBox
´´ 
.
´´ 
Show
´´ 
(
´´  
	nameError
´´  )
,
´´) *
Lang
´´+ /
.
´´/ 0
TitleValidation
´´0 ?
)
´´? @
;
´´@ A
return
¨¨ 
;
¨¨ 
}
≠≠ 
string
ÆÆ 
lastNameError
ÆÆ  
=
ÆÆ! "
SignUpValidator
ÆÆ# 2
.
ÆÆ2 3
ValidateLastName
ÆÆ3 C
(
ÆÆC D
LastName
ÆÆD L
)
ÆÆL M
;
ÆÆM N
if
ØØ 
(
ØØ 
!
ØØ 
string
ØØ 
.
ØØ 
IsNullOrEmpty
ØØ %
(
ØØ% &
lastNameError
ØØ& 3
)
ØØ3 4
)
ØØ4 5
{
∞∞ 

MessageBox
±± 
.
±± 
Show
±± 
(
±±  
lastNameError
±±  -
,
±±- .
Lang
±±/ 3
.
±±3 4
TitleValidation
±±4 C
)
±±C D
;
±±D E
return
≤≤ 
;
≤≤ 
}
≥≥ 
string
¥¥ 
nicknameError
¥¥  
=
¥¥! "
SignUpValidator
¥¥# 2
.
¥¥2 3
ValidateNickname
¥¥3 C
(
¥¥C D
Nickname
¥¥D L
)
¥¥L M
;
¥¥M N
if
µµ 
(
µµ 
!
µµ 
string
µµ 
.
µµ 
IsNullOrEmpty
µµ %
(
µµ% &
nicknameError
µµ& 3
)
µµ3 4
)
µµ4 5
{
∂∂ 

MessageBox
∑∑ 
.
∑∑ 
Show
∑∑ 
(
∑∑  
nicknameError
∑∑  -
,
∑∑- .
Lang
∑∑/ 3
.
∑∑3 4
TitleValidation
∑∑4 C
)
∑∑C D
;
∑∑D E
return
∏∏ 
;
∏∏ 
}
ππ 
playerInProgress
ªª 
.
ªª 
name
ªª !
=
ªª" #
Name
ªª$ (
.
ªª( )
Trim
ªª) -
(
ªª- .
)
ªª. /
;
ªª/ 0
playerInProgress
ºº 
.
ºº 
lastName
ºº %
=
ºº& '
LastName
ºº( 0
.
ºº0 1
Trim
ºº1 5
(
ºº5 6
)
ºº6 7
;
ºº7 8
playerInProgress
ΩΩ 
.
ΩΩ 
nickname
ΩΩ %
=
ΩΩ& '
Nickname
ΩΩ( 0
.
ΩΩ0 1
Trim
ΩΩ1 5
(
ΩΩ5 6
)
ΩΩ6 7
;
ΩΩ7 8
playerInProgress
ææ 
.
ææ 
	pathPhoto
ææ &
=
ææ' (
$str
ææ) N
;
ææN O
try
¿¿ 
{
¡¡ 
var
¬¬ 
client
¬¬ 
=
¬¬ 
new
¬¬  
SignUpClient
¬¬! -
(
¬¬- .
)
¬¬. /
;
¬¬/ 0
bool
ƒƒ &
isRegistrationSuccessful
ƒƒ -
=
ƒƒ. /
await
ƒƒ0 5
client
ƒƒ6 <
.
ƒƒ< =!
RegisterPlayerAsync
ƒƒ= P
(
ƒƒP Q
playerInProgress
ƒƒQ a
)
ƒƒa b
;
ƒƒb c
if
∆∆ 
(
∆∆ &
isRegistrationSuccessful
∆∆ ,
)
∆∆, -
{
«« 

MessageBox
»» 
.
»» 
Show
»» #
(
»»# $
Lang
»»$ (
.
»»( )#
SuccessAccountCreated
»») >
,
»»> ?
Lang
»»@ D
.
»»D E'
TitleRegistrationComplete
»»E ^
)
»»^ _
;
»»_ `$
ExecuteNavigateToLogin
…… *
(
……* +
	parameter
……+ 4
)
……4 5
;
……5 6
}
   
else
ÀÀ 
{
ÃÃ 

MessageBox
ÕÕ 
.
ÕÕ 
Show
ÕÕ #
(
ÕÕ# $
Lang
ÕÕ$ (
.
ÕÕ( )!
ErrorNicknameExists
ÕÕ) <
,
ÕÕ< =
Lang
ÕÕ> B
.
ÕÕB C$
TitleRegistrationError
ÕÕC Y
)
ÕÕY Z
;
ÕÕZ [
}
ŒŒ 
}
œœ 
catch
–– 
(
–– '
EndpointNotFoundException
–– ,
)
––, -
{
—— 

MessageBox
““ 
.
““ 
Show
““ 
(
““  
Lang
““  $
.
““$ %$
ErrorServerUnavailable
““% ;
,
““; <
Lang
““= A
.
““A B"
TitleConnectionError
““B V
)
““V W
;
““W X
}
”” 
catch
‘‘ 
(
‘‘ 
System
‘‘ 
.
‘‘ 
	Exception
‘‘ #
ex
‘‘$ &
)
‘‘& '
{
’’ 

MessageBox
÷÷ 
.
÷÷ 
Show
÷÷ 
(
÷÷  
string
÷÷  &
.
÷÷& '
Format
÷÷' -
(
÷÷- .
Lang
÷÷. 2
.
÷÷2 3%
ErrorConnectingToServer
÷÷3 J
,
÷÷J K
ex
÷÷L N
.
÷÷N O
Message
÷÷O V
)
÷÷V W
,
÷÷W X
Lang
÷÷Y ]
.
÷÷] ^"
TitleConnectionError
÷÷^ r
)
÷÷r s
;
÷÷s t
}
◊◊ 
}
ÿÿ 	
private
⁄⁄ 
static
⁄⁄ 
void
⁄⁄ $
ExecuteNavigateToLogin
⁄⁄ 2
(
⁄⁄2 3
object
⁄⁄3 9
	parameter
⁄⁄: C
)
⁄⁄C D
{
€€ 	
var
‹‹ 
loginWindow
‹‹ 
=
‹‹ 
new
‹‹ !
LogIn
‹‹" '
(
‹‹' (
)
‹‹( )
;
‹‹) *
loginWindow
›› 
.
›› 
Show
›› 
(
›› 
)
›› 
;
›› 
(
ﬁﬁ 
	parameter
ﬁﬁ 
as
ﬁﬁ 
Window
ﬁﬁ  
)
ﬁﬁ  !
?
ﬁﬁ! "
.
ﬁﬁ" #
Close
ﬁﬁ# (
(
ﬁﬁ( )
)
ﬁﬁ) *
;
ﬁﬁ* +
}
ﬂﬂ 	
private
·· 
void
·· %
ExecuteNavigateToSignUp
·· ,
(
··, -
object
··- 3
	parameter
··4 =
)
··= >
{
‚‚ 	%
EnteredVerificationCode
„„ #
=
„„$ %
string
„„& ,
.
„„, -
Empty
„„- 2
;
„„2 3
Name
‰‰ 
=
‰‰ 
string
‰‰ 
.
‰‰ 
Empty
‰‰ 
;
‰‰  
LastName
ÂÂ 
=
ÂÂ 
string
ÂÂ 
.
ÂÂ 
Empty
ÂÂ #
;
ÂÂ# $
Nickname
ÊÊ 
=
ÊÊ 
string
ÊÊ 
.
ÊÊ 
Empty
ÊÊ #
;
ÊÊ# $
var
ÁÁ 
signUpWindow
ÁÁ 
=
ÁÁ 
new
ÁÁ "
SignUp
ÁÁ# )
(
ÁÁ) *
)
ÁÁ* +
;
ÁÁ+ ,
signUpWindow
ËË 
.
ËË 
DataContext
ËË $
=
ËË% &
this
ËË' +
;
ËË+ ,
signUpWindow
ÈÈ 
.
ÈÈ 
Show
ÈÈ 
(
ÈÈ 
)
ÈÈ 
;
ÈÈ  
(
ÍÍ 
	parameter
ÍÍ 
as
ÍÍ 
Window
ÍÍ  
)
ÍÍ  !
?
ÍÍ! "
.
ÍÍ" #
Close
ÍÍ# (
(
ÍÍ( )
)
ÍÍ) *
;
ÍÍ* +
}
ÎÎ 	
}
ÏÏ 
}ÌÌ †
ÖC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\FriendList\StatusConverter.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $

FriendList$ .
{ 
public 

class 
StatusConverter  
:! "
IValueConverter# 2
{ 
public		 
object		 
Convert		 
(		 
object		 $
value		% *
,		* +
Type		, 0

targetType		1 ;
,		; <
object		= C
	parameter		D M
,		M N
CultureInfo		O Z
culture		[ b
)		b c
{

 	
if 
( 
value 
is 
int 
status #
)# $
{ 
switch 
( 
status 
) 
{ 
case 
$num 
: 
return 
$str '
;' (
case 
$num 
: 
return 
$str (
;( )
default 
: 
return 
$str ,
;, -
} 
} 
return 
$str  
;  !
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
CultureInfoS ^
culture_ f
)f g
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} ≤
åC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\FriendList\FriendProfileViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $

FriendList$ .
{		 
public

 

class

 "
FriendProfileViewModel

 '
:

( )
ViewModelBase

* 7
{ 
private 
	PlayerDto 
player  
;  !
public 
	PlayerDto 
Player 
{ 	
get 
=> 
player 
; 
set 
{ 
player 
= 
value  
;  !
OnPropertyChanged" 3
(3 4
nameof4 :
(: ;
Player; A
)A B
)B C
;C D
}E F
} 	
public 
string 
FacebookLink "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
InstagramLink #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
ICommand 
BackCommand #
{$ %
get& )
;) *
}+ ,
public "
FriendProfileViewModel %
(% &
	PlayerDto& /
player0 6
,6 7 
ObservableCollection8 L
<L M
	SocialDtoM V
>V W
socialsX _
)_ `
{ 	
Player 
= 
player 
; 
BackCommand 
= 
new 
RelayCommand *
(* +
ExecuteBackCommand+ =
)= >
;> ?
LoadSocials 
( 
socials 
)  
;  !
} 	
private   
void   
LoadSocials    
(    ! 
ObservableCollection  ! 5
<  5 6
	SocialDto  6 ?
>  ? @
socials  A H
)  H I
{!! 	
if"" 
("" 
socials"" 
!="" 
null"" 
)""  
{## 
FacebookLink$$ 
=$$ 
socials$$ &
.$$& '
FirstOrDefault$$' 5
($$5 6
s$$6 7
=>$$8 :
s$$; <
.$$< =
IdSocialType$$= I
==$$J L
$num$$M N
)$$N O
?$$O P
.$$P Q
UserLink$$Q Y
;$$Y Z
InstagramLink%% 
=%% 
socials%%  '
.%%' (
FirstOrDefault%%( 6
(%%6 7
s%%7 8
=>%%9 ;
s%%< =
.%%= >
IdSocialType%%> J
==%%K M
$num%%N O
)%%O P
?%%P Q
.%%Q R
UserLink%%R Z
;%%Z [
OnPropertyChanged'' !
(''! "
nameof''" (
(''( )
FacebookLink'') 5
)''5 6
)''6 7
;''7 8
OnPropertyChanged(( !
(((! "
nameof((" (
(((( )
InstagramLink(() 6
)((6 7
)((7 8
;((8 9
})) 
}** 	
private,, 
static,, 
void,, 
ExecuteBackCommand,, .
(,,. /
object,,/ 5
	parameter,,6 ?
),,? @
{-- 	
if.. 
(.. 
	parameter.. 
is.. 
Window.. #
window..$ *
)..* +
{// 
window00 
.00 
Close00 
(00 
)00 
;00 
}11 
}22 	
}33 
}44 ¶7
åC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\FriendList\FriendRequestViewModel.cs
	namespace

 	
Conqui√°nCliente


 
.

 
	ViewModel

 #
.

# $

FriendList

$ .
{ 
public 

class #
FriendRequestsViewModel (
:) *"
INotifyPropertyChanged+ A
{ 
public 
event '
PropertyChangedEventHandler 0
PropertyChanged1 @
;@ A
private  
ObservableCollection $
<$ %
FriendRequest% 2
>2 3
requests4 <
;< =
public  
ObservableCollection #
<# $
FriendRequest$ 1
>1 2
Requests3 ;
{ 	
get 
{ 
return 
requests !
;! "
}# $
set 
{ 
requests 
= 
value "
;" #
OnPropertyChanged$ 5
(5 6
nameof6 <
(< =
Requests= E
)E F
)F G
;G H
}I J
} 	
public 
ICommand  
AcceptRequestCommand ,
{- .
get/ 2
;2 3
}4 5
public 
ICommand !
DeclineRequestCommand -
{. /
get0 3
;3 4
}5 6
public 
ICommand 
BackCommand #
{$ %
get& )
;) *
}+ ,
private 
readonly 
FriendListClient )
FriendListService* ;
;; <
public #
FriendRequestsViewModel &
(& '
)' (
{ 	
FriendListService 
= 
new  #
FriendListClient$ 4
(4 5
)5 6
;6 7
Requests   
=   
new    
ObservableCollection   /
<  / 0
FriendRequest  0 =
>  = >
(  > ?
)  ? @
;  @ A 
AcceptRequestCommand!!  
=!!! "
new!!# &
RelayCommand!!' 3
(!!3 4
AcceptRequest!!4 A
)!!A B
;!!B C!
DeclineRequestCommand"" !
=""" #
new""$ '
RelayCommand""( 4
(""4 5
DeclineRequest""5 C
)""C D
;""D E
BackCommand## 
=## 
new## 
RelayCommand## *
(##* +
ExecuteBackCommand##+ =
)##= >
;##> ?
}$$ 	
public&& 
async&& 
Task&& 
InitializeAsync&& )
(&&) *
)&&* +
{'' 	
await(( 
LoadFriendRequests(( $
((($ %
)((% &
;((& '
})) 	
private** 
async** 
Task** 
LoadFriendRequests** -
(**- .
)**. /
{++ 	
var,, 
requestsList,, 
=,, 
await,, $
FriendListService,,% 6
.,,6 7"
GetFriendRequestsAsync,,7 M
(,,M N
PlayerSession,,N [
.,,[ \
CurrentPlayer,,\ i
.,,i j
idPlayer,,j r
),,r s
;,,s t
if-- 
(-- 
requestsList-- 
!=-- 
null--  $
)--$ %
{.. 
foreach// 
(// 
var// 
req//  
in//! #
requestsList//$ 0
)//0 1
{00 
Requests11 
.11 
Add11  
(11  !
new11! $
FriendRequest11% 2
{113 4
IdFriendship115 A
=11B C
req11D G
.11G H
IdFriendship11H T
,11T U
Nickname11V ^
=11_ `
req11a d
.11d e
Nickname11e m
}11n o
)11o p
;11p q
}22 
}33 
}44 	
private66 
async66 
void66 
AcceptRequest66 (
(66( )
object66) /
	parameter660 9
)669 :
{77 	
if88 
(88 
	parameter88 
is88 
FriendRequest88 *
request88+ 2
)882 3
{99 
bool:: 
success:: 
=:: 
await:: $
FriendListService::% 6
.::6 7*
UpdateFriendRequestStatusAsync::7 U
(::U V
request::V ]
.::] ^
IdFriendship::^ j
,::j k
$num::l m
)::m n
;::n o
if;; 
(;; 
success;; 
);; 
{<< 
Requests== 
.== 
Remove== #
(==# $
request==$ +
)==+ ,
;==, -
}>> 
}?? 
}@@ 	
privateBB 
asyncBB 
voidBB 
DeclineRequestBB )
(BB) *
objectBB* 0
	parameterBB1 :
)BB: ;
{CC 	
ifDD 
(DD 
	parameterDD 
isDD 
FriendRequestDD *
requestDD+ 2
)DD2 3
{EE 
boolFF 
successFF 
=FF 
awaitFF $
FriendListServiceFF% 6
.FF6 7*
UpdateFriendRequestStatusAsyncFF7 U
(FFU V
requestFFV ]
.FF] ^
IdFriendshipFF^ j
,FFj k
$numFFl m
)FFm n
;FFn o
ifGG 
(GG 
successGG 
)GG 
{HH 
RequestsII 
.II 
RemoveII #
(II# $
requestII$ +
)II+ ,
;II, -
}JJ 
}KK 
}LL 	
privateNN 
staticNN 
voidNN 
ExecuteBackCommandNN .
(NN. /
objectNN/ 5
	parameterNN6 ?
)NN? @
{OO 	
ifPP 
(PP 
	parameterPP 
isPP 
WindowPP #
currentWindowPP$ 1
)PP1 2
{QQ 
varRR 
friendListWindowRR $
=RR% &
newRR' *
ViewRR+ /
.RR/ 0

FriendListRR0 :
.RR: ;

FriendListRR; E
(RRE F
)RRF G
;RRG H
friendListWindowSS  
.SS  !
ShowSS! %
(SS% &
)SS& '
;SS' (
currentWindowTT 
.TT 
CloseTT #
(TT# $
)TT$ %
;TT% &
}UU 
}VV 	
	protectedXX 
voidXX 
OnPropertyChangedXX (
(XX( )
stringXX) /
propertyNameXX0 <
)XX< =
{YY 	
PropertyChangedZZ 
?ZZ 
.ZZ 
InvokeZZ #
(ZZ# $
thisZZ$ (
,ZZ( )
newZZ* -$
PropertyChangedEventArgsZZ. F
(ZZF G
propertyNameZZG S
)ZZS T
)ZZT U
;ZZU V
}[[ 	
}\\ 
}]] ©O
àC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Authentication\LogInViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
Authentication$ 2
{ 
public 

class 
LogInViewModel 
:  !
ViewModelBase" /
{ 
private 
string 
email 
; 
private 
int !
selectedLanguageIndex )
;) *
public 
string 
Email 
{ 	
get 
{ 
return 
email 
; 
}  !
set 
{ 
email 
= 
value 
;  
OnPropertyChanged! 2
(2 3
)3 4
;4 5
}6 7
} 	
public 
int !
SelectedLanguageIndex (
{ 	
get 
{ 
return !
selectedLanguageIndex .
;. /
}0 1
set 
{ !
selectedLanguageIndex %
=& '
value( -
;- .
OnPropertyChanged !
(! "
)" #
;# $
ChangeLanguage   
(   
)    
;    !
}!! 
}"" 	
public$$ 
ICommand$$ 
LoginCommand$$ $
{$$% &
get$$' *
;$$* +
}$$, -
public%% 
ICommand%% #
NavigateToSignUpCommand%% /
{%%0 1
get%%2 5
;%%5 6
}%%7 8
public&& 
ICommand&& +
NavigateToForgotPasswordCommand&& 7
{&&8 9
get&&: =
;&&= >
}&&? @
public(( 
LogInViewModel(( 
((( 
)(( 
{)) 	
LoginCommand** 
=** 
new** 
RelayCommand** +
(**+ ,
ExecuteLogin**, 8
,**8 9
CanExecuteCommand**: K
)**K L
;**L M#
NavigateToSignUpCommand++ #
=++$ %
new++& )
RelayCommand++* 6
(++6 7#
ExecuteNavigateToSignUp++7 N
)++N O
;++O P+
NavigateToForgotPasswordCommand,, +
=,,, -
new,,. 1
RelayCommand,,2 >
(,,> ?+
ExecuteNavigateToForgotPassword,,? ^
),,^ _
;,,_ `
}-- 	
private// 
static// 
bool// 
CanExecuteCommand// -
(//- .
object//. 4
	parameter//5 >
)//> ?
{00 	
return11 
true11 
;11 
}22 	
private44 
async44 
void44 
ExecuteLogin44 '
(44' (
object44( .
	parameter44/ 8
)448 9
{55 	
var66 
passwordBox66 
=66 
	parameter66 '
as66( *
PasswordBox66+ 6
;666 7
if77 
(77 
passwordBox77 
==77 
null77 #
)77# $
return77% +
;77+ ,
string88 
password88 
=88 
passwordBox88 )
.88) *
Password88* 2
;882 3
string:: 

emailError:: 
=:: 
LogInValidator::  .
.::. /
ValidateEmail::/ <
(::< =
Email::= B
)::B C
;::C D
if;; 
(;; 
!;; 
string;; 
.;; 
IsNullOrEmpty;; %
(;;% &

emailError;;& 0
);;0 1
);;1 2
{<< 

MessageBox== 
.== 
Show== 
(==  

emailError==  *
,==* +
Lang==, 0
.==0 1
TitleValidation==1 @
)==@ A
;==A B
return>> 
;>> 
}?? 
stringAA 
passwordErrorAA  
=AA! "
LogInValidatorAA# 1
.AA1 2
ValidatePasswordAA2 B
(AAB C
passwordAAC K
)AAK L
;AAL M
ifBB 
(BB 
!BB 
stringBB 
.BB 
IsNullOrEmptyBB %
(BB% &
passwordErrorBB& 3
)BB3 4
)BB4 5
{CC 

MessageBoxDD 
.DD 
ShowDD 
(DD  
passwordErrorDD  -
,DD- .
LangDD/ 3
.DD3 4
TitleValidationDD4 C
)DDC D
;DDD E
returnEE 
;EE 
}FF 
tryHH 
{II 
varJJ 
clientJJ 
=JJ 
newJJ  
LoginClientJJ! ,
(JJ, -
)JJ- .
;JJ. /
	PlayerDtoLL 
authenticatedPlayerLL -
=LL. /
awaitLL0 5
clientLL6 <
.LL< =#
AuthenticatePlayerAsyncLL= T
(LLT U
EmailLLU Z
,LLZ [
passwordLL\ d
)LLd e
;LLe f
ifNN 
(NN 
authenticatedPlayerNN '
.NN' (
idPlayerNN( 0
>NN1 2
$numNN3 4
)NN4 5
{OO 
PlayerSessionPP !
.PP! "
StartSessionPP" .
(PP. /
authenticatedPlayerPP/ B
)PPB C
;PPC D
varRR 
mainMenuRR  
=RR! "
newRR# &
ViewRR' +
.RR+ ,
MainMenuRR, 4
.RR4 5
MainMenuRR5 =
(RR= >
)RR> ?
;RR? @
mainMenuSS 
.SS 
ShowSS !
(SS! "
)SS" #
;SS# $
WindowTT 
.TT 
	GetWindowTT $
(TT$ %
passwordBoxTT% 0
)TT0 1
?TT1 2
.TT2 3
CloseTT3 8
(TT8 9
)TT9 :
;TT: ;
}UU 
elseVV 
{WW 

MessageBoxXX 
.XX 
ShowXX #
(XX# $
LangXX$ (
.XX( )#
ErrorInvalidCredentialsXX) @
,XX@ A
LangXXB F
.XXF G$
TitleAuthenticationErrorXXG _
)XX_ `
;XX` a
}YY 
}ZZ 
catch[[ 
([[ 
FaultException[[ !
<[[! "
SessionActiveFault[[" 4
>[[4 5
sessionFault[[6 B
)[[B C
{\\ 

MessageBox]] 
.]] 
Show]] 
(]]  
sessionFault]]  ,
.]], -
Detail]]- 3
.]]3 4
Message]]4 ;
,]]; <
Lang]]= A
.]]A B

TitleError]]B L
,]]L M
MessageBoxButton]]N ^
.]]^ _
OK]]_ a
,]]a b
MessageBoxImage]]c r
.]]r s
Information]]s ~
)]]~ 
;	]] Ä
}^^ 
catch__ 
(__ %
EndpointNotFoundException__ ,
)__, -
{`` 

MessageBoxaa 
.aa 
Showaa 
(aa  
Langaa  $
.aa$ %"
ErrorServerUnavailableaa% ;
,aa; <
Langaa= A
.aaA B 
TitleConnectionErroraaB V
)aaV W
;aaW X
}bb 
catchcc 
(cc 
Systemcc 
.cc 
	Exceptioncc #
excc$ &
)cc& '
{dd 

MessageBoxee 
.ee 
Showee 
(ee  
stringee  &
.ee& '
Formatee' -
(ee- .
Langee. 2
.ee2 3
ErrorUnexpectedee3 B
,eeB C
exeeD F
.eeF G
MessageeeG N
)eeN O
,eeO P
LangeeQ U
.eeU V

TitleErroreeV `
)ee` a
;eea b
}ff 
}gg 	
privateii 
staticii 
voidii #
ExecuteNavigateToSignUpii 3
(ii3 4
objectii4 :
	parameterii; D
)iiD E
{jj 	
varkk 
signUpWindowkk 
=kk 
newkk "
SignUpkk# )
(kk) *
)kk* +
;kk+ ,
signUpWindowll 
.ll 
Showll 
(ll 
)ll 
;ll  
(mm 
	parametermm 
asmm 
Windowmm  
)mm  !
?mm! "
.mm" #
Closemm# (
(mm( )
)mm) *
;mm* +
}nn 	
privatepp 
staticpp 
voidpp +
ExecuteNavigateToForgotPasswordpp ;
(pp; <
objectpp< B
	parameterppC L
)ppL M
{qq 	
varrr !
requestRecoveryWindowrr %
=rr& '
newrr( +%
PasswordRecoveryMainFramerr, E
(rrE F
)rrF G
;rrG H!
requestRecoveryWindowss !
.ss! "
Showss" &
(ss& '
)ss' (
;ss( )
(tt 
	parametertt 
astt 
Windowtt  
)tt  !
?tt! "
.tt" #
Closett# (
(tt( )
)tt) *
;tt* +
}uu 	
privateww 
voidww 
ChangeLanguageww #
(ww# $
)ww$ %
{xx 	
ifyy 
(yy !
SelectedLanguageIndexyy %
==yy& (
$numyy) *
)yy* +
{zz 

Properties{{ 
.{{ 
Settings{{ #
.{{# $
Default{{$ +
.{{+ ,
languageCode{{, 8
={{9 :
$str{{; B
;{{B C
}|| 
else}} 
if}} 
(}} !
SelectedLanguageIndex}} *
==}}+ -
$num}}. /
)}}/ 0
{~~ 

Properties 
. 
Settings #
.# $
Default$ +
.+ ,
languageCode, 8
=9 :
$str; B
;B C
}
ÄÄ 

Properties
ÅÅ 
.
ÅÅ 
Settings
ÅÅ 
.
ÅÅ  
Default
ÅÅ  '
.
ÅÅ' (
Save
ÅÅ( ,
(
ÅÅ, -
)
ÅÅ- .
;
ÅÅ. /
}
ÇÇ 	
}
ÉÉ 
}ÑÑ √
ÜC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\ImagePathToBooleanConverter.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
{ 
public 

class '
ImagePathToBooleanConverter ,
:- . 
IMultiValueConverter/ C
{ 
public		 
object		 
Convert		 
(		 
object		 $
[		$ %
]		% &
values		' -
,		- .
Type		/ 3

targetType		4 >
,		> ?
object		@ F
	parameter		G P
,		P Q
CultureInfo		R ]
culture		^ e
)		e f
{

 	
if 
( 
values 
. 
Length 
< 
$num  !
||" $
!% &
(& '
values' -
[- .
$num. /
]/ 0
is1 3
string4 :
): ;
||< >
!? @
(@ A
valuesA G
[G H
$numH I
]I J
isK M
stringN T
)T U
)U V
{ 
return 
true 
; 
} 
string 
	imagePath 
= 
values %
[% &
$num& '
]' (
as) +
string, 2
;2 3
string 
currentImagePath #
=$ %
values& ,
[, -
$num- .
]. /
as0 2
string3 9
;9 :
return 
! 
string 
. 
Equals !
(! "
	imagePath" +
,+ ,
currentImagePath- =
,= >
StringComparison? O
.O P
OrdinalIgnoreCaseP a
)a b
;b c
} 	
public 
object 
[ 
] 
ConvertBack #
(# $
object$ *
value+ 0
,0 1
Type2 6
[6 7
]7 8
targetTypes9 D
,D E
objectF L
	parameterM V
,V W
CultureInfoX c
cultured k
)k l
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} ∂ê
âC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\FriendList\FriendListViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
{ 
public 

class 
FriendListViewModel $
:% &"
INotifyPropertyChanged' =
{ 
public 
event '
PropertyChangedEventHandler 0
PropertyChanged1 @
;@ A
private  
ObservableCollection $
<$ %%
FriendInviteItemViewModel% >
>> ?
friends@ G
;G H
private  
ObservableCollection $
<$ %%
FriendInviteItemViewModel% >
>> ?
searchResult@ L
;L M
public 
ICommand 
ViewProfileCommand *
{+ ,
get- 0
;0 1
}2 3
public 
ICommand 
AddFriendCommand (
{) *
get+ .
;. /
}0 1
public 
ICommand 
RequestsCommand '
{( )
get* -
;- .
}/ 0
public 
ICommand 
BackCommand #
{$ %
get& )
;) *
}+ ,
public 
ICommand 
DeleteFriendCommand +
{, -
get. 1
;1 2
}3 4
public  
ObservableCollection #
<# $%
FriendInviteItemViewModel$ =
>= >
Friends? F
{ 	
get 
{ 
return 
friends  
;  !
}" #
set   
{   
friends   
=   
value   !
;  ! "
OnPropertyChanged  # 4
(  4 5
nameof  5 ;
(  ; <
Friends  < C
)  C D
)  D E
;  E F
}  G H
}!! 	
public##  
ObservableCollection## #
<### $%
FriendInviteItemViewModel##$ =
>##= >
SearchResult##? K
{$$ 	
get%% 
{%% 
return%% 
searchResult%% %
;%%% &
}%%' (
set&& 
{&& 
searchResult&& 
=&&  
value&&! &
;&&& '
OnPropertyChanged&&( 9
(&&9 :
nameof&&: @
(&&@ A
SearchResult&&A M
)&&M N
)&&N O
;&&O P
}&&Q R
}'' 	
private)) 
readonly)) 
FriendListClient)) )
FriendListService))* ;
;)); <
private** 
readonly** 
UserProfileClient** *
UserProfileService**+ =
;**= >
public,, 
FriendListViewModel,, "
(,," #
),,# $
{-- 	
FriendListService.. 
=.. 
new..  #
FriendListClient..$ 4
(..4 5
)..5 6
;..6 7
UserProfileService// 
=//  
new//! $
UserProfileClient//% 6
(//6 7
)//7 8
;//8 9
Friends00 
=00 
new00  
ObservableCollection00 .
<00. /%
FriendInviteItemViewModel00/ H
>00H I
(00I J
)00J K
;00K L
SearchResult11 
=11 
new11  
ObservableCollection11 3
<113 4%
FriendInviteItemViewModel114 M
>11M N
(11N O
)11O P
;11P Q
ViewProfileCommand22 
=22  
new22! $
RelayCommand22% 1
(221 2%
ExecuteViewProfileCommand222 K
)22K L
;22L M
AddFriendCommand33 
=33 
new33 "
RelayCommand33# /
(33/ 0
	AddFriend330 9
)339 :
;33: ;
RequestsCommand44 
=44 
new44 !
RelayCommand44" .
(44. /"
ExecuteRequestsCommand44/ E
)44E F
;44F G
DeleteFriendCommand55 
=55  !
new55" %
RelayCommand55& 2
(552 3
DeleteFriend553 ?
)55? @
;55@ A
BackCommand66 
=66 
new66 
RelayCommand66 *
(66* +
ExecuteBackCommand66+ =
)66= >
;66> ?
_77 
=77 
LoadFriends77 
(77 
)77 
;77 #
PresenceCallbackHandler99 #
.99# $
FriendStatusChanged99$ 7
+=998 :!
OnFriendStatusChanged99; P
;99P Q
}:: 	
private<< 
void<< !
OnFriendStatusChanged<< *
(<<* +
int<<+ .
friendId<</ 7
,<<7 8
int<<9 <
newStatusId<<= H
)<<H I
{== 	
Application>> 
.>> 
Current>> 
.>>  

Dispatcher>>  *
.>>* +
Invoke>>+ 1
(>>1 2
(>>2 3
)>>3 4
=>>>5 7
{?? 
var@@ 
friendVM@@ 
=@@ 
Friends@@ &
.@@& '
FirstOrDefault@@' 5
(@@5 6
f@@6 7
=>@@8 :
f@@; <
.@@< =
IdPlayer@@= E
==@@F H
friendId@@I Q
)@@Q R
;@@R S
ifAA 
(AA 
friendVMAA 
!=AA 
nullAA  $
)AA$ %
{BB 
boolCC 
isOnlineCC !
=CC" #
(CC$ %
newStatusIdCC% 0
==CC1 3
$numCC4 5
)CC5 6
;CC6 7
friendVMDD 
.DD 
IsOnlineDD %
=DD& '
isOnlineDD( 0
;DD0 1
friendVMEE 
.EE 

StatusTextEE '
=EE( )
isOnlineEE* 2
?EE3 4
LangEE5 9
.EE9 :
StatusOnlineEE: F
:EEG H
LangEEI M
.EEM N
StatusOfflineEEN [
;EE[ \
}FF 
varHH 
searchVMHH 
=HH 
SearchResultHH +
.HH+ ,
FirstOrDefaultHH, :
(HH: ;
fHH; <
=>HH= ?
fHH@ A
.HHA B
IdPlayerHHB J
==HHK M
friendIdHHN V
)HHV W
;HHW X
ifII 
(II 
searchVMII 
!=II 
nullII  $
)II$ %
{JJ 
boolKK 
isOnlineKK !
=KK" #
(KK$ %
newStatusIdKK% 0
==KK1 3
$numKK4 5
)KK5 6
;KK6 7
searchVMLL 
.LL 
IsOnlineLL %
=LL& '
isOnlineLL( 0
;LL0 1
searchVMMM 
.MM 

StatusTextMM '
=MM( )
isOnlineMM* 2
?MM3 4
LangMM5 9
.MM9 :
StatusOnlineMM: F
:MMG H
LangMMI M
.MMM N
StatusOfflineMMN [
;MM[ \
}NN 
}OO 
)OO 
;OO 
}PP 	
publicRR 
voidRR 
CleanupRR 
(RR 
)RR 
{SS 	#
PresenceCallbackHandlerTT #
.TT# $
FriendStatusChangedTT$ 7
-=TT8 :!
OnFriendStatusChangedTT; P
;TTP Q
}UU 	
privateWW 
asyncWW 
TaskWW 
LoadFriendsWW &
(WW& '
)WW' (
{XX 	
varYY 
friendsListYY 
=YY 
awaitYY #
FriendListServiceYY$ 5
.YY5 6
GetFriendsAsyncYY6 E
(YYE F
PlayerSessionYYF S
.YYS T
CurrentPlayerYYT a
.YYa b
idPlayerYYb j
)YYj k
;YYk l
Friends[[ 
.[[ 
Clear[[ 
([[ 
)[[ 
;[[ 
if\\ 
(\\ 
friendsList\\ 
!=\\ 
null\\ #
)\\# $
{]] 
foreach^^ 
(^^ 
var^^ 
	friendDto^^ &
in^^' )
friendsList^^* 5
.^^5 6
OrderByDescending^^6 G
(^^G H
f^^H I
=>^^J L
f^^M N
.^^N O
idStatus^^O W
)^^W X
)^^X Y
{__ 
Friends`` 
.`` 
Add`` 
(``  
new``  #%
FriendInviteItemViewModel``$ =
(``= >
	friendDto``> G
)``G H
)``H I
;``I J
}aa 
}bb 
}cc 	
publicee 
asyncee 
Taskee 
SearchPlayeree &
(ee& '
stringee' -
nicknameee. 6
)ee6 7
{ff 	
vargg 
playergg 
=gg 
awaitgg 
FriendListServicegg 0
.gg0 1$
GetPlayerByNicknameAsyncgg1 I
(ggI J
nicknameggJ R
,ggR S
PlayerSessionggT a
.gga b
CurrentPlayerggb o
.ggo p
idPlayerggp x
)ggx y
;ggy z
SearchResulthh 
.hh 
Clearhh 
(hh 
)hh  
;hh  !
ifii 
(ii 
playerii 
.ii 
idPlayerii 
>ii  !
$numii" #
)ii# $
{jj 
SearchResultkk 
.kk 
Addkk  
(kk  !
newkk! $%
FriendInviteItemViewModelkk% >
(kk> ?
playerkk? E
)kkE F
)kkF G
;kkG H
}ll 
}mm 	
privateoo 
asyncoo 
voidoo 
	AddFriendoo $
(oo$ %
objectoo% +
	parameteroo, 5
)oo5 6
{pp 	
ifqq 
(qq 
	parameterqq 
isqq %
FriendInviteItemViewModelqq 6
friendVMqq7 ?
)qq? @
{rr 
varss 
successss 
=ss 
awaitss #
FriendListServicess$ 5
.ss5 6"
SendFriendRequestAsyncss6 L
(ssL M
PlayerSessionssM Z
.ssZ [
CurrentPlayerss[ h
.ssh i
idPlayerssi q
,ssq r
friendVMsss {
.ss{ |
IdPlayer	ss| Ñ
)
ssÑ Ö
;
ssÖ Ü
iftt 
(tt 
successtt 
)tt 
{uu 

MessageBoxvv 
.vv 
Showvv #
(vv# $
Langvv$ (
.vv( )$
FriendRequestSentSuccessvv) A
,vvA B
LangvvC G
.vvG H
TitleSuccessvvH T
)vvT U
;vvU V
}ww 
elsexx 
{yy 

MessageBoxzz 
.zz 
Showzz #
(zz# $
Langzz$ (
.zz( )"
FriendRequestSentErrorzz) ?
,zz? @
LangzzA E
.zzE F

TitleErrorzzF P
)zzP Q
;zzQ R
}{{ 
}|| 
}}} 	
private 
static 
void "
ExecuteRequestsCommand 2
(2 3
object3 9
	parameter: C
)C D
{
ÄÄ 	
if
ÅÅ 
(
ÅÅ 
	parameter
ÅÅ 
is
ÅÅ 
Window
ÅÅ #
currentWindow
ÅÅ$ 1
)
ÅÅ1 2
{
ÇÇ 
var
ÉÉ 
requestsWindow
ÉÉ "
=
ÉÉ# $
new
ÉÉ% (
View
ÉÉ) -
.
ÉÉ- .

FriendList
ÉÉ. 8
.
ÉÉ8 9
FriendRequests
ÉÉ9 G
(
ÉÉG H
)
ÉÉH I
;
ÉÉI J
requestsWindow
ÑÑ 
.
ÑÑ 
Show
ÑÑ #
(
ÑÑ# $
)
ÑÑ$ %
;
ÑÑ% &
currentWindow
ÖÖ 
.
ÖÖ 
Close
ÖÖ #
(
ÖÖ# $
)
ÖÖ$ %
;
ÖÖ% &
}
ÜÜ 
}
áá 	
private
ââ 
static
ââ 
void
ââ  
ExecuteBackCommand
ââ .
(
ââ. /
object
ââ/ 5
	parameter
ââ6 ?
)
ââ? @
{
ää 	
if
ãã 
(
ãã 
	parameter
ãã 
is
ãã 
Window
ãã #
currentWindow
ãã$ 1
)
ãã1 2
{
åå 
var
çç 
mainMenu
çç 
=
çç 
new
çç "
View
çç# '
.
çç' (
MainMenu
çç( 0
.
çç0 1
MainMenu
çç1 9
(
çç9 :
)
çç: ;
;
çç; <
mainMenu
éé 
.
éé 
Show
éé 
(
éé 
)
éé 
;
éé  
currentWindow
èè 
.
èè 
Close
èè #
(
èè# $
)
èè$ %
;
èè% &
}
êê 
}
ëë 	
private
ïï 
async
ïï 
void
ïï '
ExecuteViewProfileCommand
ïï 4
(
ïï4 5
object
ïï5 ;
	parameter
ïï< E
)
ïïE F
{
ññ 	
if
óó 
(
óó 
	parameter
óó 
is
óó '
FriendInviteItemViewModel
óó 6
friendVM
óó7 ?
)
óó? @
{
òò 
try
ôô 
{
öö 
var
õõ 
fullPlayerProfile
õõ )
=
õõ* +
await
õõ, 1 
UserProfileService
õõ2 D
.
õõD E 
GetPlayerByIdAsync
õõE W
(
õõW X
friendVM
õõX `
.
õõ` a
IdPlayer
õõa i
)
õõi j
;
õõj k
var
úú 
socials
úú 
=
úú  !
await
úú" ' 
UserProfileService
úú( :
.
úú: ;#
GetPlayerSocialsAsync
úú; P
(
úúP Q
friendVM
úúQ Y
.
úúY Z
IdPlayer
úúZ b
)
úúb c
;
úúc d
if
ûû 
(
ûû 
fullPlayerProfile
ûû )
!=
ûû* ,
null
ûû- 1
)
ûû1 2
{
üü 
var
†† 
profileWindow
†† )
=
††* +
new
††, /
FriendProfile
††0 =
(
††= >
fullPlayerProfile
††> O
,
††O P
new
††Q T"
ObservableCollection
††U i
<
††i j 
ServiceUserProfile
††j |
.
††| }
	SocialDto††} Ü
>††Ü á
(††á à
socials††à è
)††è ê
)††ê ë
;††ë í
profileWindow
°° %
.
°°% &

ShowDialog
°°& 0
(
°°0 1
)
°°1 2
;
°°2 3
}
¢¢ 
else
££ 
{
§§ 

MessageBox
•• "
.
••" #
Show
••# '
(
••' (
$str
••( R
,
••R S
$str
••T [
,
••[ \
MessageBoxButton
••] m
.
••m n
OK
••n p
,
••p q
MessageBoxImage••r Å
.••Å Ç
Error••Ç á
)••á à
;••à â
}
¶¶ 
}
ßß 
catch
®® 
(
®® 
System
®® 
.
®® 
	Exception
®® '
ex
®®( *
)
®®* +
{
©© 

MessageBox
™™ 
.
™™ 
Show
™™ #
(
™™# $
$str
™™$ P
,
™™P Q
$str
™™R e
,
™™e f
MessageBoxButton
™™g w
.
™™w x
OK
™™x z
,
™™z {
MessageBoxImage™™| ã
.™™ã å
Error™™å ë
)™™ë í
;™™í ì
}
´´ 
}
¨¨ 
}
≠≠ 	
private
ØØ 
async
ØØ 
void
ØØ 
DeleteFriend
ØØ '
(
ØØ' (
object
ØØ( .
	parameter
ØØ/ 8
)
ØØ8 9
{
∞∞ 	
if
±± 
(
±± 
	parameter
±± 
is
±± '
FriendInviteItemViewModel
±± 6
friendVM
±±7 ?
)
±±? @
{
≤≤ 
MessageBoxResult
≥≥  
result
≥≥! '
=
≥≥( )

MessageBox
≥≥* 4
.
≥≥4 5
Show
≥≥5 9
(
≥≥9 :
string
≥≥: @
.
≥≥@ A
Format
≥≥A G
(
≥≥G H
Lang
≥≥H L
.
≥≥L M*
FriendListDeleteConfirmation
≥≥M i
,
≥≥i j
friendVM
≥≥k s
.
≥≥s t
Nickname
≥≥t |
)
≥≥| }
,
≥≥} ~
Lang≥≥ É
.≥≥É Ñ!
TitleConfirmation≥≥Ñ ï
,≥≥ï ñ 
MessageBoxButton≥≥ó ß
.≥≥ß ®
YesNo≥≥® ≠
,≥≥≠ Æ
MessageBoxImage≥≥Ø æ
.≥≥æ ø
Question≥≥ø «
)≥≥« »
;≥≥» …
if
¥¥ 
(
¥¥ 
result
¥¥ 
==
¥¥ 
MessageBoxResult
¥¥ .
.
¥¥. /
Yes
¥¥/ 2
)
¥¥2 3
{
µµ 
var
∂∂ 
success
∂∂ 
=
∂∂  !
await
∂∂" '
FriendListService
∂∂( 9
.
∂∂9 :
DeleteFriendAsync
∂∂: K
(
∂∂K L
PlayerSession
∂∂L Y
.
∂∂Y Z
CurrentPlayer
∂∂Z g
.
∂∂g h
idPlayer
∂∂h p
,
∂∂p q
friendVM
∂∂r z
.
∂∂z {
IdPlayer∂∂{ É
)∂∂É Ñ
;∂∂Ñ Ö
if
∑∑ 
(
∑∑ 
success
∑∑ 
)
∑∑  
{
∏∏ 
Friends
ππ 
.
ππ  
Remove
ππ  &
(
ππ& '
friendVM
ππ' /
)
ππ/ 0
;
ππ0 1

MessageBox
∫∫ "
.
∫∫" #
Show
∫∫# '
(
∫∫' (
Lang
∫∫( ,
.
∫∫, -&
FriendListDeletedSuccess
∫∫- E
,
∫∫E F
Lang
∫∫G K
.
∫∫K L
TitleSuccess
∫∫L X
)
∫∫X Y
;
∫∫Y Z
}
ªª 
else
ºº 
{
ΩΩ 

MessageBox
ææ "
.
ææ" #
Show
ææ# '
(
ææ' (
Lang
ææ( ,
.
ææ, -$
FriendListDeletedError
ææ- C
,
ææC D
Lang
ææE I
.
ææI J

TitleError
ææJ T
)
ææT U
;
ææU V
}
øø 
}
¿¿ 
}
¡¡ 
}
¬¬ 	
	protected
ƒƒ 
void
ƒƒ 
OnPropertyChanged
ƒƒ (
(
ƒƒ( )
string
ƒƒ) /
propertyName
ƒƒ0 <
)
ƒƒ< =
{
≈≈ 	
PropertyChanged
∆∆ 
?
∆∆ 
.
∆∆ 
Invoke
∆∆ #
(
∆∆# $
this
∆∆$ (
,
∆∆( )
new
∆∆* -&
PropertyChangedEventArgs
∆∆. F
(
∆∆F G
propertyName
∆∆G S
)
∆∆S T
)
∆∆T U
;
∆∆U V
}
«« 	
}
»» 
}…… Ê¿
§C:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\ViewModel\Authentication\PasswordRecovery\PasswordRecoveryViewModel.cs
	namespace 	
Conqui√°nCliente
 
. 
	ViewModel #
.# $
Authentication$ 2
.2 3
PasswordRecovery3 C
{ 
public 

enum 
PasswordUpdateMode "
{ 
Recovery 
= 
$num 
, 
Change 
= 
$num 
} 
public 

class %
PasswordRecoveryViewModel *
:+ ,
ViewModelBase- :
{ 
private 
string 
email 
; 
private 
string 
token 
; 
private 
string 
newPassword "
;" #
private 
string 
confirmPassword &
;& '
private 
bool 
	isLoading 
; 
private 
readonly 
IPasswordRecovery *
recoveryClient+ 9
;9 :
private 
PasswordUpdateMode "
_mode# (
;( )
public 
PasswordUpdateMode !
Mode" &
{   	
get!! 
=>!! 
_mode!! 
;!! 
set"" 
{## 
_mode$$ 
=$$ 
value$$ 
;$$ 
OnPropertyChanged%% !
(%%! "
nameof%%" (
(%%( )
Mode%%) -
)%%- .
)%%. /
;%%/ 0
OnPropertyChanged&& !
(&&! "
nameof&&" (
(&&( )
	PageTitle&&) 2
)&&2 3
)&&3 4
;&&4 5
}'' 
}(( 	
public** 
bool** 
IsEditProfileFlow** %
{**& '
get**( +
;**+ ,
set**- 0
;**0 1
}**2 3
=**4 5
false**6 ;
;**; <
public++ 
string++ 
	PageTitle++ 
{,, 	
get-- 
{.. 
return// 
Mode// 
==// 
PasswordUpdateMode// 1
.//1 2
Change//2 8
?00 
Lang00 
.00 
EditDataEdit00 '
:11 
Lang11 
.11 "
GlobalPasswordRecovery11 1
;111 2
}22 
}33 	
public44 
string44 
Email44 
{55 	
get66 
=>66 
email66 
;66 
set77 
{77 
email77 
=77 
value77 
;77  
OnPropertyChanged77! 2
(772 3
nameof773 9
(779 :
Email77: ?
)77? @
)77@ A
;77A B
}77C D
}88 	
public99 
string99 
Token99 
{:: 	
get;; 
=>;; 
token;; 
;;; 
set<< 
{<< 
token<< 
=<< 
value<< 
;<<  
OnPropertyChanged<<! 2
(<<2 3
nameof<<3 9
(<<9 :
Token<<: ?
)<<? @
)<<@ A
;<<A B
}<<C D
}== 	
public>> 
string>> 
NewPassword>> !
{?? 	
get@@ 
=>@@ 
newPassword@@ 
;@@ 
setAA 
{AA 
newPasswordAA 
=AA 
valueAA  %
;AA% &
OnPropertyChangedAA' 8
(AA8 9
nameofAA9 ?
(AA? @
NewPasswordAA@ K
)AAK L
)AAL M
;AAM N
}AAO P
}BB 	
publicCC 
stringCC 
ConfirmPasswordCC %
{DD 	
getEE 
=>EE 
confirmPasswordEE "
;EE" #
setFF 
{FF 
confirmPasswordFF !
=FF" #
valueFF$ )
;FF) *
OnPropertyChangedFF+ <
(FF< =
nameofFF= C
(FFC D
ConfirmPasswordFFD S
)FFS T
)FFT U
;FFU V
}FFW X
}GG 	
publicHH 
boolHH 
	IsLoadingHH 
{II 	
getJJ 
=>JJ 
	isLoadingJJ 
;JJ 
setKK 
{LL 
	isLoadingMM 
=MM 
valueMM !
;MM! "
OnPropertyChangedNN !
(NN! "
nameofNN" (
(NN( )
	IsLoadingNN) 2
)NN2 3
)NN3 4
;NN4 5
CommandManagerOO 
.OO &
InvalidateRequerySuggestedOO 9
(OO9 :
)OO: ;
;OO; <
}PP 
}QQ 	
publicSS 
ICommandSS "
RequestRecoveryCommandSS .
{SS/ 0
getSS1 4
;SS4 5
}SS6 7
publicTT 
ICommandTT  
ValidateTokenCommandTT ,
{TT- .
getTT/ 2
;TT2 3
}TT4 5
publicUU 
ICommandUU  
ResetPasswordCommandUU ,
{UU- .
getUU/ 2
;UU2 3
}UU4 5
publicVV 
ICommandVV "
NavigateToLoginCommandVV .
{VV/ 0
getVV1 4
;VV4 5
}VV6 7
publicWW 
ICommandWW "
NavigateToStartCommandWW .
{WW/ 0
getWW1 4
;WW4 5
}WW6 7
publicYY %
PasswordRecoveryViewModelYY (
(YY( )
)YY) *
{ZZ 	
Mode[[ 
=[[ 
PasswordUpdateMode[[ %
.[[% &
Recovery[[& .
;[[. /"
RequestRecoveryCommand\\ "
=\\# $
new\\% (
RelayCommand\\) 5
(\\5 6"
ExecuteRequestRecovery\\6 L
,\\L M
CanExecuteCommand\\N _
)\\_ `
;\\` a 
ValidateTokenCommand]]  
=]]! "
new]]# &
RelayCommand]]' 3
(]]3 4 
ExecuteValidateToken]]4 H
,]]H I
CanExecuteCommand]]J [
)]][ \
;]]\ ] 
ResetPasswordCommand^^  
=^^! "
new^^# &
RelayCommand^^' 3
(^^3 4 
ExecuteResetPassword^^4 H
,^^H I
CanExecuteCommand^^J [
)^^[ \
;^^\ ]"
NavigateToLoginCommand__ "
=__# $
new__% (
RelayCommand__) 5
(__5 6"
ExecuteNavigateToLogin__6 L
)__L M
;__M N"
NavigateToStartCommand`` "
=``# $
new``% (
RelayCommand``) 5
(``5 6"
ExecuteNavigateToStart``6 L
,``L M%
CanExecuteNavigateToStart``N g
)``g h
;``h i
trybb 
{cc 
recoveryClientdd 
=dd  
newdd! $"
PasswordRecoveryClientdd% ;
(dd; <
)dd< =
;dd= >
}ee 
catchff 
(ff 
	Exceptionff 
exff 
)ff  
{gg 

MessageBoxhh 
.hh 
Showhh 
(hh  
stringii 
.ii 
Formatii !
(ii! "
Langii" &
.ii& '#
ErrorConnectingToServerii' >
,ii> ?
exii@ B
.iiB C
MessageiiC J
)iiJ K
,iiK L
LangiiM Q
.iiQ R 
TitleConnectionErroriiR f
)iif g
;iig h
}jj 
}kk 	
privatemm 
boolmm 
CanExecuteCommandmm &
(mm& '
objectmm' -
	parametermm. 7
)mm7 8
{nn 	
returnoo 
!oo 
	IsLoadingoo 
;oo 
}pp 	
privaterr 
asyncrr 
voidrr "
ExecuteRequestRecoveryrr 1
(rr1 2
objectrr2 8
	parameterrr9 B
)rrB C
{ss 	
thistt 
.tt 
Modett 
=tt 
PasswordUpdateModett *
.tt* +
Recoverytt+ 3
;tt3 4
stringvv 
validationErrorvv "
=vv# $%
PasswordRecoveryValidatorvv% >
.vv> ?
ValidateEmailvv? L
(vvL M
EmailvvM R
)vvR S
;vvS T
ifww 
(ww 
!ww 
stringww 
.ww 
IsNullOrEmptyww %
(ww% &
validationErrorww& 5
)ww5 6
)ww6 7
{xx 

MessageBoxyy 
.yy 
Showyy 
(yy  
validationErroryy  /
,yy/ 0
Langyy1 5
.yy5 6
TitleValidationyy6 E
)yyE F
;yyF G
returnzz 
;zz 
}{{ 
bool}} 
success}} 
=}} 
await}}  !
TryExecuteServiceCall}}! 6
(}}6 7
(~~ 
)~~ 
=>~~ 
recoveryClient~~ $
.~~$ %(
RequestPasswordRecoveryAsync~~% A
(~~A B
Email~~B G
,~~G H
(~~I J
int~~J M
)~~M N
this~~N R
.~~R S
Mode~~S W
)~~W X
,~~X Y
Lang 
. &
ErrorRecoveryRequestFailed /
)
ÄÄ 
;
ÄÄ 
if
ÇÇ 
(
ÇÇ 
success
ÇÇ 
)
ÇÇ 
{
ÉÉ 
var
ÑÑ 
page
ÑÑ 
=
ÑÑ 
	parameter
ÑÑ $
as
ÑÑ% '
Page
ÑÑ( ,
;
ÑÑ, -
page
ÖÖ 
?
ÖÖ 
.
ÖÖ 
NavigationService
ÖÖ '
?
ÖÖ' (
.
ÖÖ( )
Navigate
ÖÖ) 1
(
ÖÖ1 2
new
ÖÖ2 5
CodeValidation
ÖÖ6 D
(
ÖÖD E
this
ÖÖE I
)
ÖÖI J
)
ÖÖJ K
;
ÖÖK L
}
ÜÜ 
}
áá 	
public
ââ 
async
ââ 
Task
ââ 
<
ââ 
bool
ââ 
>
ââ -
RequestChangePasswordTokenAsync
ââ  ?
(
ââ? @
)
ââ@ A
{
ää 	
this
ãã 
.
ãã 
Mode
ãã 
=
ãã  
PasswordUpdateMode
ãã *
.
ãã* +
Change
ãã+ 1
;
ãã1 2
bool
åå 
success
åå 
=
åå 
await
åå  #
TryExecuteServiceCall
åå! 6
(
åå6 7
(
çç 
)
çç 
=>
çç 
recoveryClient
çç $
.
çç$ %*
RequestPasswordRecoveryAsync
çç% A
(
ççA B
Email
ççB G
,
ççG H
(
ççI J
int
ççJ M
)
ççM N
this
ççN R
.
ççR S
Mode
ççS W
)
ççW X
,
ççX Y
Lang
éé 
.
éé (
ErrorRecoveryRequestFailed
éé /
)
èè 
;
èè 
return
ëë 
success
ëë 
;
ëë 
}
íí 	
private
ìì 
async
ìì 
void
ìì "
ExecuteValidateToken
ìì /
(
ìì/ 0
object
ìì0 6
	parameter
ìì7 @
)
ìì@ A
{
îî 	
string
ïï 
validationError
ïï "
=
ïï# $'
PasswordRecoveryValidator
ïï% >
.
ïï> ?
ValidateToken
ïï? L
(
ïïL M
Token
ïïM R
)
ïïR S
;
ïïS T
if
ññ 
(
ññ 
!
ññ 
string
ññ 
.
ññ 
IsNullOrEmpty
ññ %
(
ññ% &
validationError
ññ& 5
)
ññ5 6
)
ññ6 7
{
óó 

MessageBox
òò 
.
òò 
Show
òò 
(
òò  
validationError
òò  /
,
òò/ 0
Lang
òò1 5
.
òò5 6
TitleValidation
òò6 E
)
òòE F
;
òòF G
return
ôô 
;
ôô 
}
öö 
bool
úú 
success
úú 
=
úú 
await
úú  #
TryExecuteServiceCall
úú! 6
(
úú6 7
(
ùù 
)
ùù 
=>
ùù 
recoveryClient
ùù $
.
ùù$ %(
ValidateRecoveryTokenAsync
ùù% ?
(
ùù? @
Email
ùù@ E
,
ùùE F
Token
ùùG L
)
ùùL M
,
ùùM N
Lang
ûû 
.
ûû ,
ErrorVerificationCodeIncorrect
ûû 3
)
üü 
;
üü 
if
°° 
(
°° 
success
°° 
)
°° 
{
¢¢ 
var
££ 
page
££ 
=
££ 
	parameter
££ $
as
££% '
Page
££( ,
;
££, -
page
§§ 
?
§§ 
.
§§ 
NavigationService
§§ '
?
§§' (
.
§§( )
Navigate
§§) 1
(
§§1 2
new
§§2 5
ResetPassword
§§6 C
(
§§C D
this
§§D H
)
§§H I
)
§§I J
;
§§J K
}
•• 
}
¶¶ 	
private
®® 
async
®® 
void
®® "
ExecuteResetPassword
®® /
(
®®/ 0
object
®®0 6
	parameter
®®7 @
)
®®@ A
{
©© 	
var
™™ 
page
™™ 
=
™™ 
	parameter
™™  
as
™™! #
Page
™™$ (
;
™™( )
string
¨¨ 
validationError
¨¨ "
=
¨¨# $'
PasswordRecoveryValidator
¨¨% >
.
¨¨> ?
ValidatePasswords
¨¨? P
(
¨¨P Q
NewPassword
¨¨Q \
,
¨¨\ ]
ConfirmPassword
¨¨^ m
)
¨¨m n
;
¨¨n o
if
ÆÆ 
(
ÆÆ 
!
ÆÆ 
string
ÆÆ 
.
ÆÆ 
IsNullOrEmpty
ÆÆ %
(
ÆÆ% &
validationError
ÆÆ& 5
)
ÆÆ5 6
)
ÆÆ6 7
{
ØØ 

MessageBox
∞∞ 
.
∞∞ 
Show
∞∞ 
(
∞∞  
validationError
∞∞  /
,
∞∞/ 0
Lang
∞∞1 5
.
∞∞5 6
TitleValidation
∞∞6 E
)
∞∞E F
;
∞∞F G
return
±± 
;
±± 
}
≤≤ 
try
¥¥ 
{
µµ 
	IsLoading
∂∂ 
=
∂∂ 
true
∂∂  
;
∂∂  !
var
∑∑ 
client
∑∑ 
=
∑∑ 
new
∑∑  %
ServicePasswordRecovery
∑∑! 8
.
∑∑8 9$
PasswordRecoveryClient
∑∑9 O
(
∑∑O P
)
∑∑P Q
;
∑∑Q R
bool
∏∏ 
success
∏∏ 
=
∏∏ 
await
∏∏ $
client
∏∏% +
.
∏∏+ , 
ResetPasswordAsync
∏∏, >
(
∏∏> ?
Email
∏∏? D
,
∏∏D E
Token
∏∏F K
,
∏∏K L
newPassword
∏∏M X
)
∏∏X Y
;
∏∏Y Z
if
∫∫ 
(
∫∫ 
success
∫∫ 
)
∫∫ 
{
ªª 
if
ºº 
(
ºº 
IsEditProfileFlow
ºº )
)
ºº) *
{
ΩΩ 
page
ææ 
?
ææ 
.
ææ 
NavigationService
ææ /
?
ææ/ 0
.
ææ0 1
Navigate
ææ1 9
(
ææ9 :
new
ææ: =
UserProfilePage
ææ> M
(
ææM N
)
ææN O
)
ææO P
;
ææP Q
}
øø 
else
¿¿ 
{
¡¡ 
var
¬¬ 
loginWindow
¬¬ '
=
¬¬( )
new
¬¬* -
LogIn
¬¬. 3
(
¬¬3 4
)
¬¬4 5
;
¬¬5 6
loginWindow
√√ #
.
√√# $
Show
√√$ (
(
√√( )
)
√√) *
;
√√* +
var
≈≈ 
currentWindow
≈≈ )
=
≈≈* +
Window
≈≈, 2
.
≈≈2 3
	GetWindow
≈≈3 <
(
≈≈< =
page
≈≈= A
)
≈≈A B
;
≈≈B C
if
«« 
(
«« 
currentWindow
«« )
!=
««* ,
null
««- 1
)
««1 2
{
»» 
currentWindow
…… )
.
……) *
Close
……* /
(
……/ 0
)
……0 1
;
……1 2
}
   
}
ÀÀ 
}
ÃÃ 
else
ÕÕ 
{
ŒŒ 

MessageBox
œœ 
.
œœ 
Show
œœ #
(
œœ# $
Lang
œœ$ (
.
œœ( )(
ErrorRecoveryRequestFailed
œœ) C
,
œœC D
Lang
œœE I
.
œœI J

TitleError
œœJ T
)
œœT U
;
œœU V
}
–– 
}
—— 
catch
““ 
(
““ '
EndpointNotFoundException
““ ,
)
““, -
{
”” 

MessageBox
‘‘ 
.
‘‘ 
Show
‘‘ 
(
‘‘  
Lang
‘‘  $
.
‘‘$ %$
ErrorServerUnavailable
‘‘% ;
,
‘‘; <
Lang
‘‘= A
.
‘‘A B"
TitleConnectionError
‘‘B V
)
‘‘V W
;
‘‘W X
}
’’ 
catch
÷÷ 
(
÷÷ 
FaultException
÷÷ !
ex
÷÷" $
)
÷÷$ %
{
◊◊ 

MessageBox
ÿÿ 
.
ÿÿ 
Show
ÿÿ 
(
ÿÿ  
string
ÿÿ  &
.
ÿÿ& '
Format
ÿÿ' -
(
ÿÿ- .
Lang
ÿÿ. 2
.
ÿÿ2 3
ErrorUnexpected
ÿÿ3 B
,
ÿÿB C
ex
ÿÿD F
.
ÿÿF G
Message
ÿÿG N
)
ÿÿN O
,
ÿÿO P
Lang
ÿÿQ U
.
ÿÿU V

TitleError
ÿÿV `
)
ÿÿ` a
;
ÿÿa b
}
ŸŸ 
finally
⁄⁄ 
{
€€ 
	IsLoading
‹‹ 
=
‹‹ 
false
‹‹ !
;
‹‹! "
}
›› 
}
ﬁﬁ 	
private
‡‡ 
bool
‡‡ '
CanExecuteNavigateToStart
‡‡ .
(
‡‡. /
object
‡‡/ 5
	parameter
‡‡6 ?
)
‡‡? @
{
·· 	
return
‚‚ 
!
‚‚ 
	IsLoading
‚‚ 
;
‚‚ 
}
„„ 	
private
‰‰ 
void
‰‰ $
ExecuteNavigateToStart
‰‰ +
(
‰‰+ ,
object
‰‰, 2
	parameter
‰‰3 <
)
‰‰< =
{
ÂÂ 	
this
ÊÊ 
.
ÊÊ 
Token
ÊÊ 
=
ÊÊ 
string
ÊÊ 
.
ÊÊ  
Empty
ÊÊ  %
;
ÊÊ% &
this
ÁÁ 
.
ÁÁ 
NewPassword
ÁÁ 
=
ÁÁ 
string
ÁÁ %
.
ÁÁ% &
Empty
ÁÁ& +
;
ÁÁ+ ,
this
ËË 
.
ËË 
ConfirmPassword
ËË  
=
ËË! "
string
ËË# )
.
ËË) *
Empty
ËË* /
;
ËË/ 0
var
ÈÈ 
page
ÈÈ 
=
ÈÈ 
	parameter
ÈÈ  
as
ÈÈ! #
Page
ÈÈ$ (
;
ÈÈ( )
if
ÎÎ 
(
ÎÎ 
IsEditProfileFlow
ÎÎ !
)
ÎÎ! "
{
ÏÏ 
if
ÌÌ 
(
ÌÌ 
page
ÌÌ 
?
ÌÌ 
.
ÌÌ 
NavigationService
ÌÌ +
?
ÌÌ+ ,
.
ÌÌ, -
	CanGoBack
ÌÌ- 6
==
ÌÌ7 9
true
ÌÌ: >
)
ÌÌ> ?
{
ÓÓ 
page
ÔÔ 
.
ÔÔ 
NavigationService
ÔÔ *
.
ÔÔ* +
GoBack
ÔÔ+ 1
(
ÔÔ1 2
)
ÔÔ2 3
;
ÔÔ3 4
}
 
}
ÒÒ 
else
ÚÚ 
{
ÛÛ 
page
ÙÙ 
?
ÙÙ 
.
ÙÙ 
NavigationService
ÙÙ '
?
ÙÙ' (
.
ÙÙ( )
Navigate
ÙÙ) 1
(
ÙÙ1 2
new
ÙÙ2 5
RequestRecovery
ÙÙ6 E
(
ÙÙE F
)
ÙÙF G
)
ÙÙG H
;
ÙÙH I
}
ıı 
}
ˆˆ 	
private
˜˜ 
static
˜˜ 
void
˜˜ $
ExecuteNavigateToLogin
˜˜ 2
(
˜˜2 3
object
˜˜3 9
	parameter
˜˜: C
)
˜˜C D
{
¯¯ 	
var
˘˘ 
page
˘˘ 
=
˘˘ 
	parameter
˘˘  
as
˘˘! #
Page
˘˘$ (
;
˘˘( )
var
˙˙ 
window
˙˙ 
=
˙˙ 
Window
˙˙ 
.
˙˙  
	GetWindow
˙˙  )
(
˙˙) *
page
˙˙* .
)
˙˙. /
;
˙˙/ 0
var
¸¸ 
loginWindow
¸¸ 
=
¸¸ 
new
¸¸ !
LogIn
¸¸" '
(
¸¸' (
)
¸¸( )
;
¸¸) *
loginWindow
˝˝ 
.
˝˝ 
Show
˝˝ 
(
˝˝ 
)
˝˝ 
;
˝˝ 
window
˛˛ 
?
˛˛ 
.
˛˛ 
Close
˛˛ 
(
˛˛ 
)
˛˛ 
;
˛˛ 
}
ˇˇ 	
private
ÅÅ 
async
ÅÅ 
Task
ÅÅ 
<
ÅÅ 
bool
ÅÅ 
>
ÅÅ  #
TryExecuteServiceCall
ÅÅ! 6
(
ÅÅ6 7
Func
ÅÅ7 ;
<
ÅÅ; <
Task
ÅÅ< @
<
ÅÅ@ A
bool
ÅÅA E
>
ÅÅE F
>
ÅÅF G
serviceCall
ÅÅH S
,
ÅÅS T
string
ÅÅU ["
businessErrorMessage
ÅÅ\ p
)
ÅÅp q
{
ÇÇ 	
if
ÉÉ 
(
ÉÉ 
recoveryClient
ÉÉ 
==
ÉÉ !
null
ÉÉ" &
)
ÉÉ& '
{
ÑÑ 

MessageBox
ÖÖ 
.
ÖÖ 
Show
ÖÖ 
(
ÖÖ  
Lang
ÖÖ  $
.
ÖÖ$ %%
ErrorConnectingToServer
ÖÖ% <
,
ÖÖ< =
Lang
ÖÖ> B
.
ÖÖB C

TitleError
ÖÖC M
)
ÖÖM N
;
ÖÖN O
return
ÜÜ 
false
ÜÜ 
;
ÜÜ 
}
áá 
	IsLoading
ââ 
=
ââ 
true
ââ 
;
ââ 
try
ää 
{
ãã 
bool
åå 
success
åå 
=
åå 
await
åå $
serviceCall
åå% 0
(
åå0 1
)
åå1 2
;
åå2 3
if
éé 
(
éé 
!
éé 
success
éé 
)
éé 
{
èè 

MessageBox
êê 
.
êê 
Show
êê #
(
êê# $"
businessErrorMessage
êê$ 8
,
êê8 9
Lang
êê: >
.
êê> ?

TitleError
êê? I
)
êêI J
;
êêJ K
}
ëë 
return
íí 
success
íí 
;
íí 
}
ìì 
catch
îî 
(
îî 
	Exception
îî 
ex
îî 
)
îî  
{
ïï 
HandleException
ññ 
(
ññ  
ex
ññ  "
)
ññ" #
;
ññ# $
return
óó 
false
óó 
;
óó 
}
òò 
finally
ôô 
{
öö 
	IsLoading
õõ 
=
õõ 
false
õõ !
;
õõ! "
}
úú 
}
ùù 	
private
üü 
static
üü 
void
üü 
HandleException
üü +
(
üü+ ,
	Exception
üü, 5
ex
üü6 8
)
üü8 9
{
†† 	
if
°° 
(
°° 
ex
°° 
is
°° '
EndpointNotFoundException
°° /
)
°°/ 0
{
¢¢ 

MessageBox
££ 
.
££ 
Show
££ 
(
££  
Lang
££  $
.
££$ %$
ErrorServerUnavailable
££% ;
,
££; <
Lang
££= A
.
££A B"
TitleConnectionError
££B V
)
££V W
;
££W X
}
§§ 
else
•• 
if
•• 
(
•• 
ex
•• 
is
•• $
CommunicationException
•• 1
)
••1 2
{
¶¶ 

MessageBox
ßß 
.
ßß 
Show
ßß 
(
ßß  
string
ßß  &
.
ßß& '
Format
ßß' -
(
ßß- .
Lang
ßß. 2
.
ßß2 3%
ErrorConnectingToServer
ßß3 J
,
ßßJ K
ex
ßßL N
.
ßßN O
Message
ßßO V
)
ßßV W
,
ßßW X
Lang
ßßY ]
.
ßß] ^"
TitleConnectionError
ßß^ r
)
ßßr s
;
ßßs t
}
®® 
else
©© 
if
©© 
(
©© 
ex
©© 
is
©© 
TimeoutException
©© +
)
©©+ ,
{
™™ 

MessageBox
´´ 
.
´´ 
Show
´´ 
(
´´  
Lang
´´  $
.
´´$ %
ErrorTimeout
´´% 1
,
´´1 2
Lang
´´3 7
.
´´7 8"
TitleConnectionError
´´8 L
)
´´L M
;
´´M N
}
¨¨ 
else
≠≠ 
{
ÆÆ 

MessageBox
ØØ 
.
ØØ 
Show
ØØ 
(
ØØ  
string
ØØ  &
.
ØØ& '
Format
ØØ' -
(
ØØ- .
Lang
ØØ. 2
.
ØØ2 3
ErrorGeneric
ØØ3 ?
,
ØØ? @
ex
ØØA C
.
ØØC D
Message
ØØD K
)
ØØK L
,
ØØL M
Lang
ØØN R
.
ØØR S

TitleError
ØØS ]
)
ØØ] ^
;
ØØ^ _
}
∞∞ 
}
±± 	
}
≤≤ 
}≥≥ Ä
uC:\Tecnologias para el desarrollo de software\Cliente\TCS-ConquianGameClient\Conqui√°nCliente\Models\FriendRequest.cs
	namespace 	
Conqui√°nCliente
 
. 
Models  
{ 
public		 

class		 
FriendRequest		 
{

 
public 
int 
IdFriendship 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
Nickname 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} 