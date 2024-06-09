import {ReactElement} from "react";
import {
    Activity,
    ArrowUpRight,
    CreditCard,
    DollarSign,
    Users,
} from "lucide-react"

import {
    Avatar,
    AvatarFallback,
    AvatarImage,
} from "@/components/ui/avatar"
import {Badge} from "@/components/ui/badge"
import {Button} from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"
import {Link} from "react-router-dom";

/**
 * Strona dashboardu
 */
export default function DashboardPage(): ReactElement {
    return (
        <main className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
            <div className="grid gap-4 md:grid-cols-2 md:gap-8 lg:grid-cols-4">
                <Card x-chunk="dashboard-01-chunk-0">
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                            Number of Active Campaigns
                        </CardTitle>
                        <DollarSign className="h-4 w-4 text-muted-foreground"/>
                    </CardHeader>
                    <CardContent>
                        <div className="text-2xl font-bold">
                            4
                        </div>
                        <p className="text-xs text-muted-foreground">
                            +1 from last month
                        </p>
                    </CardContent>
                </Card>
                <Card x-chunk="dashboard-01-chunk-1">
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                            Messages Sent Total
                        </CardTitle>
                        <Users className="h-4 w-4 text-muted-foreground"/>
                    </CardHeader>
                    <CardContent>
                        <div className="text-2xl font-bold">2350</div>
                        <p className="text-xs text-muted-foreground">
                            +150 since last week
                        </p>
                    </CardContent>
                </Card>
                <Card x-chunk="dashboard-01-chunk-2">
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                            Created Characters
                        </CardTitle>
                        <CreditCard className="h-4 w-4 text-muted-foreground"/>
                    </CardHeader>
                    <CardContent>
                        <div className="text-2xl font-bold">
                            235
                        </div>
                        <p className="text-xs text-muted-foreground">
                            +23 since last month
                        </p>
                    </CardContent>
                </Card>
                <Card x-chunk="dashboard-01-chunk-3">
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                            Active Users
                        </CardTitle>
                        <Activity className="h-4 w-4 text-muted-foreground"/>
                    </CardHeader>
                    <CardContent>
                        <div className="text-2xl font-bold">212</div>
                        <p className="text-xs text-muted-foreground">
                            From 2821 registered total
                        </p>
                    </CardContent>
                </Card>
            </div>
            <div className="grid gap-4 md:gap-8 lg:grid-cols-2 xl:grid-cols-3">
                <Card
                    className="xl:col-span-2" x-chunk="dashboard-01-chunk-4"
                >
                    <CardHeader className="flex flex-row items-center">
                        <div className="grid gap-2">
                            <CardTitle>Active Campaigns</CardTitle>
                            <CardDescription>
                                A list of all active campaigns you participate in
                            </CardDescription>
                        </div>
                        <Button asChild size="sm" className="ml-auto gap-1">
                            <Link to="#">
                                View All
                                <ArrowUpRight className="h-4 w-4"/>
                            </Link>
                        </Button>
                    </CardHeader>
                    <CardContent>
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Campaign</TableHead>
                                    <TableHead className="text-right">
                                        Last Message Date
                                    </TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                <TableRow>
                                    <TableCell>
                                        <div className="font-medium">
                                            <Link to={"#"} className="hover:underline">
                                                Świat Apokalipsy - Przygoda
                                            </Link>
                                        </div>
                                        <div className="hidden text-sm text-muted-foreground md:inline">
                                            hosted by&nbsp;
                                            <Link to={"#"} className="hover:underline">
                                                John Doe
                                            </Link>
                                        </div>
                                    </TableCell>
                                    <TableCell className="text-right">
                                        2023-06-23
                                    </TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </CardContent>
                </Card>
                <Card x-chunk="dashboard-01-chunk-5">
                    <CardHeader>
                        <CardTitle>Recent Messages</CardTitle>
                    </CardHeader>
                    <CardContent className="grid gap-8">
                        <div className="flex items-center gap-4">
                            <Avatar className="hidden h-9 w-9 sm:flex">
                                <AvatarImage src="/avatars/01.png" alt="Avatar"/>
                                <AvatarFallback>OM</AvatarFallback>
                            </Avatar>
                            <div className="grid gap-1">
                                <p className="text-sm font-medium leading-none">
                                    <b>Olivia Martin</b> as <i className="font-normal">Jane "The Explorer"</i>
                                </p>
                                <p className="text-xs text-muted-foreground">
                                    Przed wyruszeniem na wyprawę, musiałam...
                                </p>
                            </div>
                            <div className="ml-auto font-normal">
                                <Badge variant={"default"}>New</Badge>
                                <p className="text-xs text-muted-foreground">2023-06-23</p>
                            </div>
                        </div>
                        <div className="flex items-center gap-4">
                            <Avatar className="hidden h-9 w-9 sm:flex">
                                <AvatarImage src="/avatars/02.png" alt="Avatar"/>
                                <AvatarFallback>JL</AvatarFallback>
                            </Avatar>
                            <div className="grid gap-1">
                                <p className="text-sm font-medium leading-none">
                                    <b>Jackson Lee</b> as <i className="font-normal">John "The Brave"</i>
                                </p>
                                <p className="text-xs text-muted-foreground">
                                    <em>I'm ready to face the dragon! Who's with...</em>
                                </p>
                            </div>
                            <div className="ml-auto font-medium">
                                <p className="text-xs text-muted-foreground">2023-06-23</p>
                                <p className="text-xs text-muted-foreground">2 days ago</p>
                            </div>
                        </div>
                    </CardContent>
                </Card>
            </div>
        </main>
    )
}