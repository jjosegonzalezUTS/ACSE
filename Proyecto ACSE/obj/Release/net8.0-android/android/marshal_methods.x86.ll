; ModuleID = 'marshal_methods.x86.ll'
source_filename = "marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [127 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [254 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 101
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 100
	i32 42639949, ; 2: System.Threading.Thread => 0x28aa24d => 118
	i32 67008169, ; 3: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 33
	i32 72070932, ; 4: Microsoft.Maui.Graphics.dll => 0x44bb714 => 47
	i32 117431740, ; 5: System.Runtime.InteropServices => 0x6ffddbc => 112
	i32 126191764, ; 6: Proyecto ACSE => 0x7858894 => 81
	i32 165246403, ; 7: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 57
	i32 182336117, ; 8: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 75
	i32 195452805, ; 9: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 30
	i32 199333315, ; 10: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 31
	i32 205061960, ; 11: System.ComponentModel => 0xc38ff48 => 89
	i32 280992041, ; 12: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 2
	i32 317674968, ; 13: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 30
	i32 318968648, ; 14: Xamarin.AndroidX.Activity.dll => 0x13031348 => 53
	i32 336156722, ; 15: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 15
	i32 342366114, ; 16: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 64
	i32 347068432, ; 17: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 51
	i32 356389973, ; 18: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 14
	i32 375677976, ; 19: System.Net.ServicePoint.dll => 0x16646418 => 105
	i32 379916513, ; 20: System.Threading.Thread.dll => 0x16a510e1 => 118
	i32 385762202, ; 21: System.Memory.dll => 0x16fe439a => 97
	i32 395744057, ; 22: _Microsoft.Android.Resource.Designer => 0x17969339 => 34
	i32 435591531, ; 23: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 26
	i32 442565967, ; 24: System.Collections => 0x1a61054f => 85
	i32 450948140, ; 25: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 63
	i32 469710990, ; 26: System.dll => 0x1bff388e => 121
	i32 498788369, ; 27: System.ObjectModel => 0x1dbae811 => 108
	i32 500358224, ; 28: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 13
	i32 503918385, ; 29: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 7
	i32 513247710, ; 30: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 42
	i32 539058512, ; 31: Microsoft.Extensions.Logging => 0x20216150 => 39
	i32 592146354, ; 32: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 21
	i32 627609679, ; 33: Xamarin.AndroidX.CustomView => 0x2568904f => 61
	i32 627931235, ; 34: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 19
	i32 672442732, ; 35: System.Collections.Concurrent => 0x2814a96c => 82
	i32 683518922, ; 36: System.Net.Security => 0x28bdabca => 104
	i32 688181140, ; 37: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 1
	i32 706645707, ; 38: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 16
	i32 709557578, ; 39: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 4
	i32 722857257, ; 40: System.Runtime.Loader.dll => 0x2b15ed29 => 113
	i32 748832960, ; 41: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 49
	i32 759454413, ; 42: System.Net.Requests => 0x2d445acd => 103
	i32 775507847, ; 43: System.IO.Compression => 0x2e394f87 => 94
	i32 777317022, ; 44: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 25
	i32 789151979, ; 45: Microsoft.Extensions.Options => 0x2f0980eb => 41
	i32 823281589, ; 46: System.Private.Uri.dll => 0x311247b5 => 109
	i32 830298997, ; 47: System.IO.Compression.Brotli => 0x317d5b75 => 93
	i32 904024072, ; 48: System.ComponentModel.Primitives.dll => 0x35e25008 => 87
	i32 926902833, ; 49: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 28
	i32 967690846, ; 50: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 64
	i32 992768348, ; 51: System.Collections.dll => 0x3b2c715c => 85
	i32 1012816738, ; 52: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 74
	i32 1028951442, ; 53: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 38
	i32 1029334545, ; 54: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 3
	i32 1035644815, ; 55: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 54
	i32 1044663988, ; 56: System.Linq.Expressions.dll => 0x3e444eb4 => 95
	i32 1052210849, ; 57: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 66
	i32 1082857460, ; 58: System.ComponentModel.TypeConverter => 0x408b17f4 => 88
	i32 1084122840, ; 59: Xamarin.Kotlin.StdLib => 0x409e66d8 => 79
	i32 1098259244, ; 60: System => 0x41761b2c => 121
	i32 1118262833, ; 61: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 16
	i32 1168523401, ; 62: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 22
	i32 1178241025, ; 63: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 71
	i32 1203215381, ; 64: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 20
	i32 1234928153, ; 65: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 18
	i32 1260983243, ; 66: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 2
	i32 1292207520, ; 67: SQLitePCLRaw.core.dll => 0x4d0585a0 => 50
	i32 1293217323, ; 68: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 62
	i32 1324164729, ; 69: System.Linq => 0x4eed2679 => 96
	i32 1373134921, ; 70: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 32
	i32 1376866003, ; 71: Xamarin.AndroidX.SavedState => 0x52114ed3 => 74
	i32 1406073936, ; 72: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 58
	i32 1430672901, ; 73: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 0
	i32 1452070440, ; 74: System.Formats.Asn1.dll => 0x568cd628 => 92
	i32 1458022317, ; 75: System.Net.Security.dll => 0x56e7a7ad => 104
	i32 1461004990, ; 76: es\Microsoft.Maui.Controls.resources => 0x57152abe => 6
	i32 1462112819, ; 77: System.IO.Compression.dll => 0x57261233 => 94
	i32 1469204771, ; 78: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 55
	i32 1470490898, ; 79: Microsoft.Extensions.Primitives => 0x57a5e912 => 42
	i32 1480492111, ; 80: System.IO.Compression.Brotli.dll => 0x583e844f => 93
	i32 1493001747, ; 81: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 10
	i32 1514721132, ; 82: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 5
	i32 1543031311, ; 83: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 117
	i32 1551623176, ; 84: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 25
	i32 1622152042, ; 85: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 68
	i32 1624863272, ; 86: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 77
	i32 1636350590, ; 87: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 60
	i32 1639515021, ; 88: System.Net.Http.dll => 0x61b9038d => 98
	i32 1639986890, ; 89: System.Text.RegularExpressions => 0x61c036ca => 117
	i32 1641389582, ; 90: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 86
	i32 1657153582, ; 91: System.Runtime => 0x62c6282e => 115
	i32 1658251792, ; 92: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 78
	i32 1677501392, ; 93: System.Net.Primitives.dll => 0x63fca3d0 => 102
	i32 1679769178, ; 94: System.Security.Cryptography => 0x641f3e5a => 116
	i32 1711441057, ; 95: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 51
	i32 1729485958, ; 96: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 56
	i32 1736233607, ; 97: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 23
	i32 1743415430, ; 98: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 1
	i32 1766324549, ; 99: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 75
	i32 1770582343, ; 100: Microsoft.Extensions.Logging.dll => 0x6988f147 => 39
	i32 1780572499, ; 101: Mono.Android.Runtime.dll => 0x6a216153 => 125
	i32 1782862114, ; 102: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 17
	i32 1788241197, ; 103: Xamarin.AndroidX.Fragment => 0x6a96652d => 63
	i32 1793755602, ; 104: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 9
	i32 1808609942, ; 105: Xamarin.AndroidX.Loader => 0x6bcd3296 => 68
	i32 1813058853, ; 106: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 79
	i32 1813201214, ; 107: Xamarin.Google.Android.Material => 0x6c13413e => 78
	i32 1818569960, ; 108: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 72
	i32 1822114437, ; 109: Proyecto ACSE.dll => 0x6c9b4285 => 81
	i32 1828688058, ; 110: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 40
	i32 1842015223, ; 111: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 29
	i32 1853025655, ; 112: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 26
	i32 1858542181, ; 113: System.Linq.Expressions => 0x6ec71a65 => 95
	i32 1875935024, ; 114: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 8
	i32 1910275211, ; 115: System.Collections.NonGeneric.dll => 0x71dc7c8b => 83
	i32 1968388702, ; 116: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 35
	i32 2003115576, ; 117: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 5
	i32 2019465201, ; 118: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 66
	i32 2025202353, ; 119: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 0
	i32 2045470958, ; 120: System.Private.Xml => 0x79eb68ee => 110
	i32 2055257422, ; 121: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 65
	i32 2066184531, ; 122: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 4
	i32 2079903147, ; 123: System.Runtime.dll => 0x7bf8cdab => 115
	i32 2090596640, ; 124: System.Numerics.Vectors => 0x7c9bf920 => 107
	i32 2103459038, ; 125: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 52
	i32 2127167465, ; 126: System.Console => 0x7ec9ffe9 => 90
	i32 2142473426, ; 127: System.Collections.Specialized => 0x7fb38cd2 => 84
	i32 2159891885, ; 128: Microsoft.Maui => 0x80bd55ad => 45
	i32 2169148018, ; 129: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 12
	i32 2181898931, ; 130: Microsoft.Extensions.Options.dll => 0x820d22b3 => 41
	i32 2192057212, ; 131: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 40
	i32 2193016926, ; 132: System.ObjectModel.dll => 0x82b6c85e => 108
	i32 2201107256, ; 133: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 80
	i32 2201231467, ; 134: System.Net.Http => 0x8334206b => 98
	i32 2207618523, ; 135: it\Microsoft.Maui.Controls.resources => 0x839595db => 14
	i32 2266799131, ; 136: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 36
	i32 2270573516, ; 137: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 8
	i32 2279755925, ; 138: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 73
	i32 2295906218, ; 139: System.Net.Sockets => 0x88d8bfaa => 106
	i32 2298471582, ; 140: System.Net.Mail => 0x88ffe49e => 99
	i32 2303942373, ; 141: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 18
	i32 2305521784, ; 142: System.Private.CoreLib.dll => 0x896b7878 => 123
	i32 2340441535, ; 143: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 111
	i32 2353062107, ; 144: System.Net.Primitives => 0x8c40e0db => 102
	i32 2368005991, ; 145: System.Xml.ReaderWriter.dll => 0x8d24e767 => 120
	i32 2371007202, ; 146: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 35
	i32 2395872292, ; 147: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 13
	i32 2427813419, ; 148: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 10
	i32 2435356389, ; 149: System.Console.dll => 0x912896e5 => 90
	i32 2458678730, ; 150: System.Net.Sockets.dll => 0x928c75ca => 106
	i32 2465273461, ; 151: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 49
	i32 2471841756, ; 152: netstandard.dll => 0x93554fdc => 122
	i32 2475788418, ; 153: Java.Interop.dll => 0x93918882 => 124
	i32 2480646305, ; 154: Microsoft.Maui.Controls => 0x93dba8a1 => 43
	i32 2483903535, ; 155: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 86
	i32 2484371297, ; 156: System.Net.ServicePoint => 0x94147f61 => 105
	i32 2550873716, ; 157: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 11
	i32 2593496499, ; 158: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 20
	i32 2605712449, ; 159: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 80
	i32 2617129537, ; 160: System.Private.Xml.dll => 0x9bfe3a41 => 110
	i32 2620871830, ; 161: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 60
	i32 2626831493, ; 162: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 15
	i32 2663698177, ; 163: System.Runtime.Loader => 0x9ec4cf01 => 113
	i32 2724373263, ; 164: System.Runtime.Numerics.dll => 0xa262a30f => 114
	i32 2732626843, ; 165: Xamarin.AndroidX.Activity => 0xa2e0939b => 53
	i32 2737747696, ; 166: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 55
	i32 2752995522, ; 167: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 21
	i32 2758225723, ; 168: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 44
	i32 2764765095, ; 169: Microsoft.Maui.dll => 0xa4caf7a7 => 45
	i32 2778768386, ; 170: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 76
	i32 2785988530, ; 171: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 27
	i32 2801831435, ; 172: Microsoft.Maui.Graphics => 0xa7008e0b => 47
	i32 2806116107, ; 173: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 6
	i32 2810250172, ; 174: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 58
	i32 2831556043, ; 175: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 19
	i32 2853208004, ; 176: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 76
	i32 2861189240, ; 177: Microsoft.Maui.Essentials => 0xaa8a4878 => 46
	i32 2909740682, ; 178: System.Private.CoreLib => 0xad6f1e8a => 123
	i32 2916838712, ; 179: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 77
	i32 2919462931, ; 180: System.Numerics.Vectors.dll => 0xae037813 => 107
	i32 2959614098, ; 181: System.ComponentModel.dll => 0xb0682092 => 89
	i32 2978675010, ; 182: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 62
	i32 3038032645, ; 183: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 34
	i32 3057625584, ; 184: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 69
	i32 3059408633, ; 185: Mono.Android.Runtime => 0xb65adef9 => 125
	i32 3059793426, ; 186: System.ComponentModel.Primitives => 0xb660be12 => 87
	i32 3077302341, ; 187: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 12
	i32 3103600923, ; 188: System.Formats.Asn1 => 0xb8fd311b => 92
	i32 3178803400, ; 189: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 70
	i32 3220365878, ; 190: System.Threading => 0xbff2e236 => 119
	i32 3258312781, ; 191: Xamarin.AndroidX.CardView => 0xc235e84d => 56
	i32 3286872994, ; 192: SQLite-net.dll => 0xc3e9b3a2 => 48
	i32 3305363605, ; 193: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 7
	i32 3316684772, ; 194: System.Net.Requests.dll => 0xc5b097e4 => 103
	i32 3317135071, ; 195: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 61
	i32 3346324047, ; 196: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 71
	i32 3357674450, ; 197: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 24
	i32 3360279109, ; 198: SQLitePCLRaw.core => 0xc849ca45 => 50
	i32 3362522851, ; 199: Xamarin.AndroidX.Core => 0xc86c06e3 => 59
	i32 3366347497, ; 200: Java.Interop => 0xc8a662e9 => 124
	i32 3374999561, ; 201: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 73
	i32 3381016424, ; 202: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 3
	i32 3428513518, ; 203: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 37
	i32 3430777524, ; 204: netstandard => 0xcc7d82b4 => 122
	i32 3463511458, ; 205: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 11
	i32 3471940407, ; 206: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 88
	i32 3476120550, ; 207: Mono.Android => 0xcf3163e6 => 126
	i32 3479583265, ; 208: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 24
	i32 3484440000, ; 209: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 23
	i32 3580758918, ; 210: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 31
	i32 3608519521, ; 211: System.Linq.dll => 0xd715a361 => 96
	i32 3624195450, ; 212: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 111
	i32 3641597786, ; 213: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 65
	i32 3643446276, ; 214: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 28
	i32 3643854240, ; 215: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 70
	i32 3657292374, ; 216: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 36
	i32 3660523487, ; 217: System.Net.NetworkInformation => 0xda2f27df => 101
	i32 3672681054, ; 218: Mono.Android.dll => 0xdae8aa5e => 126
	i32 3697841164, ; 219: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 33
	i32 3724971120, ; 220: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 69
	i32 3732100267, ; 221: System.Net.NameResolution => 0xde7354ab => 100
	i32 3748608112, ; 222: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 91
	i32 3754567612, ; 223: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 52
	i32 3786282454, ; 224: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 57
	i32 3792276235, ; 225: System.Collections.NonGeneric => 0xe2098b0b => 83
	i32 3802395368, ; 226: System.Collections.Specialized.dll => 0xe2a3f2e8 => 84
	i32 3823082795, ; 227: System.Security.Cryptography.dll => 0xe3df9d2b => 116
	i32 3841636137, ; 228: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 38
	i32 3844307129, ; 229: System.Net.Mail.dll => 0xe52378b9 => 99
	i32 3849253459, ; 230: System.Runtime.InteropServices.dll => 0xe56ef253 => 112
	i32 3876362041, ; 231: SQLite-net => 0xe70c9739 => 48
	i32 3889960447, ; 232: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 32
	i32 3896106733, ; 233: System.Collections.Concurrent.dll => 0xe839deed => 82
	i32 3896760992, ; 234: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 59
	i32 3928044579, ; 235: System.Xml.ReaderWriter => 0xea213423 => 120
	i32 3931092270, ; 236: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 72
	i32 3955647286, ; 237: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 54
	i32 3980434154, ; 238: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 27
	i32 3987592930, ; 239: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 9
	i32 4025784931, ; 240: System.Memory => 0xeff49a63 => 97
	i32 4046471985, ; 241: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 44
	i32 4073602200, ; 242: System.Threading.dll => 0xf2ce3c98 => 119
	i32 4094352644, ; 243: Microsoft.Maui.Essentials.dll => 0xf40add04 => 46
	i32 4100113165, ; 244: System.Private.Uri => 0xf462c30d => 109
	i32 4102112229, ; 245: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 22
	i32 4125707920, ; 246: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 17
	i32 4126470640, ; 247: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 37
	i32 4150914736, ; 248: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 29
	i32 4182413190, ; 249: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 67
	i32 4213026141, ; 250: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 91
	i32 4271975918, ; 251: Microsoft.Maui.Controls.dll => 0xfea12dee => 43
	i32 4274976490, ; 252: System.Runtime.Numerics => 0xfecef6ea => 114
	i32 4292120959 ; 253: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 67
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [254 x i32] [
	i32 101, ; 0
	i32 100, ; 1
	i32 118, ; 2
	i32 33, ; 3
	i32 47, ; 4
	i32 112, ; 5
	i32 81, ; 6
	i32 57, ; 7
	i32 75, ; 8
	i32 30, ; 9
	i32 31, ; 10
	i32 89, ; 11
	i32 2, ; 12
	i32 30, ; 13
	i32 53, ; 14
	i32 15, ; 15
	i32 64, ; 16
	i32 51, ; 17
	i32 14, ; 18
	i32 105, ; 19
	i32 118, ; 20
	i32 97, ; 21
	i32 34, ; 22
	i32 26, ; 23
	i32 85, ; 24
	i32 63, ; 25
	i32 121, ; 26
	i32 108, ; 27
	i32 13, ; 28
	i32 7, ; 29
	i32 42, ; 30
	i32 39, ; 31
	i32 21, ; 32
	i32 61, ; 33
	i32 19, ; 34
	i32 82, ; 35
	i32 104, ; 36
	i32 1, ; 37
	i32 16, ; 38
	i32 4, ; 39
	i32 113, ; 40
	i32 49, ; 41
	i32 103, ; 42
	i32 94, ; 43
	i32 25, ; 44
	i32 41, ; 45
	i32 109, ; 46
	i32 93, ; 47
	i32 87, ; 48
	i32 28, ; 49
	i32 64, ; 50
	i32 85, ; 51
	i32 74, ; 52
	i32 38, ; 53
	i32 3, ; 54
	i32 54, ; 55
	i32 95, ; 56
	i32 66, ; 57
	i32 88, ; 58
	i32 79, ; 59
	i32 121, ; 60
	i32 16, ; 61
	i32 22, ; 62
	i32 71, ; 63
	i32 20, ; 64
	i32 18, ; 65
	i32 2, ; 66
	i32 50, ; 67
	i32 62, ; 68
	i32 96, ; 69
	i32 32, ; 70
	i32 74, ; 71
	i32 58, ; 72
	i32 0, ; 73
	i32 92, ; 74
	i32 104, ; 75
	i32 6, ; 76
	i32 94, ; 77
	i32 55, ; 78
	i32 42, ; 79
	i32 93, ; 80
	i32 10, ; 81
	i32 5, ; 82
	i32 117, ; 83
	i32 25, ; 84
	i32 68, ; 85
	i32 77, ; 86
	i32 60, ; 87
	i32 98, ; 88
	i32 117, ; 89
	i32 86, ; 90
	i32 115, ; 91
	i32 78, ; 92
	i32 102, ; 93
	i32 116, ; 94
	i32 51, ; 95
	i32 56, ; 96
	i32 23, ; 97
	i32 1, ; 98
	i32 75, ; 99
	i32 39, ; 100
	i32 125, ; 101
	i32 17, ; 102
	i32 63, ; 103
	i32 9, ; 104
	i32 68, ; 105
	i32 79, ; 106
	i32 78, ; 107
	i32 72, ; 108
	i32 81, ; 109
	i32 40, ; 110
	i32 29, ; 111
	i32 26, ; 112
	i32 95, ; 113
	i32 8, ; 114
	i32 83, ; 115
	i32 35, ; 116
	i32 5, ; 117
	i32 66, ; 118
	i32 0, ; 119
	i32 110, ; 120
	i32 65, ; 121
	i32 4, ; 122
	i32 115, ; 123
	i32 107, ; 124
	i32 52, ; 125
	i32 90, ; 126
	i32 84, ; 127
	i32 45, ; 128
	i32 12, ; 129
	i32 41, ; 130
	i32 40, ; 131
	i32 108, ; 132
	i32 80, ; 133
	i32 98, ; 134
	i32 14, ; 135
	i32 36, ; 136
	i32 8, ; 137
	i32 73, ; 138
	i32 106, ; 139
	i32 99, ; 140
	i32 18, ; 141
	i32 123, ; 142
	i32 111, ; 143
	i32 102, ; 144
	i32 120, ; 145
	i32 35, ; 146
	i32 13, ; 147
	i32 10, ; 148
	i32 90, ; 149
	i32 106, ; 150
	i32 49, ; 151
	i32 122, ; 152
	i32 124, ; 153
	i32 43, ; 154
	i32 86, ; 155
	i32 105, ; 156
	i32 11, ; 157
	i32 20, ; 158
	i32 80, ; 159
	i32 110, ; 160
	i32 60, ; 161
	i32 15, ; 162
	i32 113, ; 163
	i32 114, ; 164
	i32 53, ; 165
	i32 55, ; 166
	i32 21, ; 167
	i32 44, ; 168
	i32 45, ; 169
	i32 76, ; 170
	i32 27, ; 171
	i32 47, ; 172
	i32 6, ; 173
	i32 58, ; 174
	i32 19, ; 175
	i32 76, ; 176
	i32 46, ; 177
	i32 123, ; 178
	i32 77, ; 179
	i32 107, ; 180
	i32 89, ; 181
	i32 62, ; 182
	i32 34, ; 183
	i32 69, ; 184
	i32 125, ; 185
	i32 87, ; 186
	i32 12, ; 187
	i32 92, ; 188
	i32 70, ; 189
	i32 119, ; 190
	i32 56, ; 191
	i32 48, ; 192
	i32 7, ; 193
	i32 103, ; 194
	i32 61, ; 195
	i32 71, ; 196
	i32 24, ; 197
	i32 50, ; 198
	i32 59, ; 199
	i32 124, ; 200
	i32 73, ; 201
	i32 3, ; 202
	i32 37, ; 203
	i32 122, ; 204
	i32 11, ; 205
	i32 88, ; 206
	i32 126, ; 207
	i32 24, ; 208
	i32 23, ; 209
	i32 31, ; 210
	i32 96, ; 211
	i32 111, ; 212
	i32 65, ; 213
	i32 28, ; 214
	i32 70, ; 215
	i32 36, ; 216
	i32 101, ; 217
	i32 126, ; 218
	i32 33, ; 219
	i32 69, ; 220
	i32 100, ; 221
	i32 91, ; 222
	i32 52, ; 223
	i32 57, ; 224
	i32 83, ; 225
	i32 84, ; 226
	i32 116, ; 227
	i32 38, ; 228
	i32 99, ; 229
	i32 112, ; 230
	i32 48, ; 231
	i32 32, ; 232
	i32 82, ; 233
	i32 59, ; 234
	i32 120, ; 235
	i32 72, ; 236
	i32 54, ; 237
	i32 27, ; 238
	i32 9, ; 239
	i32 97, ; 240
	i32 44, ; 241
	i32 119, ; 242
	i32 46, ; 243
	i32 109, ; 244
	i32 22, ; 245
	i32 17, ; 246
	i32 37, ; 247
	i32 29, ; 248
	i32 67, ; 249
	i32 91, ; 250
	i32 43, ; 251
	i32 114, ; 252
	i32 67 ; 253
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.2xx @ 0d97e20b84d8e87c3502469ee395805907905fe3"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"NumRegisterParameters", i32 0}
