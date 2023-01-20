import { animated, useSpring } from "react-spring";
import {
    BumpPoint,
    BumpDatum,
    BumpSerieExtraProps,
    ResponsiveBump
} from "@nivo/bump";
import { useState } from "react";
import game_state from "../../src/game_state/game_state.json";
import type { MilaResultsDTO } from "../../src/game_state/gameState";

interface PlayerStandingsChartProps {
    results: MilaResultsDTO;
}

interface CustomBumpDatum extends BumpDatum {
points: number;
}

const CustomPoint: React.FC<{
point: BumpPoint<CustomBumpDatum, BumpSerieExtraProps>;
}> = ({ point }) => {

const animatedProps = useSpring<{
    x: number;
    y: number;
    radius: number;
    color: string;
    borderColor: string;
    borderWidth: number;
    backgroundRadius: number;
}>({
    x: point.x,
    y: point.y,
    radius: point.size / 2,
    backgroundRadius: point.size / 2 - point.borderWidth,
    color: point.color,
    borderColor: point.borderColor,
    borderWidth: point.borderWidth,
    config: { mass: 1, tension: 170, friction: 26, clamp: true },
});

return (
    <animated.g
        transform={`translate(${point.x}, ${point.y ?? 0})`}
        style={{ pointerEvents: "none" }}
    >
    <animated.circle
        r={animatedProps.radius}
        fill={animatedProps.borderColor}
        stroke={animatedProps.borderColor}
        strokeWidth={animatedProps.borderWidth}
    />
    <animated.circle r={animatedProps.backgroundRadius} fill="white" />
    {point.size !== 0 && (
        <animated.text
        textAnchor="middle"
        fontSize="10px"
        fill={animatedProps.borderColor}
        dy=".3em"
        >
        {point.data.points}
        </animated.text>
    )}
    </animated.g>
);
};

const GetFriendlyName = (name: string, isXs: boolean): string => {
return isXs ? `${name.substring(0, 10)}..` : name;
};

const PlayerStandingsChart: React.FC<PlayerStandingsChartProps> = ({
    results
  }: PlayerStandingsChartProps) => {
    const [week, setWeek] = useState<number[]>([
      results.resultsByWeek.length - 5,
      results.resultsByWeek.length
    ]);
  
    const data = results.resultsByUser.map((r, i) => ({
      id: r.teamName,
      data: r.results.slice(week[0], week[1]).map((rr) => ({
        x: `GW ${rr.gameWeek}`,
        y: rr.milaRank,
        points: rr.cumulativeAverageMilaPoints
      }))
    }));
  
    return (
        <div style={{height: "55vh"}}>
            <ResponsiveBump
                data={data}
                xOuterPadding={0.3}
                theme={{ fontSize: 12 }}
                colors={{ scheme: "category10" }}
                lineWidth={5}
                activeLineWidth={7}
                inactiveLineWidth={5}
                inactiveOpacity={0.15}
                startLabel={false}
                pointSize={28}
                activePointSize={31}
                inactivePointSize={0}
                pointColor={{ theme: "background" }}
                pointBorderWidth={3}
                activePointBorderWidth={3}
                pointBorderColor={{ from: "serie.color" }}
                pointComponent={CustomPoint}
                enableGridY={false}
                axisTop={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: "",
                    legendPosition: "middle",
                    legendOffset: -36
                }}
                axisBottom={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: "",
                    legendPosition: "middle",
                    legendOffset: 32
                }}
                axisLeft={null}
                margin={{
                    top: 40,
                    right: 150, // 90 for SM,
                    bottom: 40,
                    left: 10
                }}
                axisRight={null}
            />
        </div>
    );
};

const Trend = () => {
    const milaResults: MilaResultsDTO = game_state;
    
    return(
        <PlayerStandingsChart results={milaResults}/>
    )
};

export default Trend;