<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<html>
			<head>
				<title>Список студентів</title>
				<meta charset="utf-8"/>
				<style>
					body { font-family: Arial, sans-serif; }
					table { border-collapse: collapse; width: 100%; margin-top: 20px; }
					th, td { border: 1px solid #999; padding: 8px; text-align: left; vertical-align: top; }
					th { background-color: #eee; }
					tr:nth-child(even) { background-color: #f9f9f9; }
				</style>
			</head>
			<body>
				<h1>Список студентів гуртожитку</h1>
				<table>
					<tr>
						<th>П.І.П.</th>
						<th>Факультет / Кафедра</th>
						<th>Курс</th>
						<th>Адреса проживання</th>
					</tr>

					<xsl:for-each select="Dormitories/Student">
						<tr>
							<td>
								<b>
									<xsl:value-of select="Name/Last"/>
								</b>
								<br/>
								<xsl:value-of select="Name/First"/>
								<xsl:if test="Name/Patronymic">
									<xsl:text> </xsl:text>
									<xsl:value-of select="Name/Patronymic"/>
								</xsl:if>
								<br/>
								<span style="font-size:0.9em; color:#555;">
									Статус: <xsl:value-of select="@status"/>
								</span>
							</td>

							<td>
								<xsl:value-of select="Faculty/Name"/>
								<br/>
								<i>
									<xsl:value-of select="Chair"/>
								</i>
								<xsl:if test="Faculty/Specialty">
									<br/>
									<span style="font-size:0.9em; color:#555;">
										Спеціальність: <xsl:value-of select="Faculty/Specialty"/>
									</span>
								</xsl:if>
							</td>

							<td>
								<xsl:value-of select="Course"/>
							</td>

							<td>
								<xsl:value-of select="Residence/Address"/>
								<br/>
								<span style="font-size:0.8em; color:#666;">
									(<xsl:value-of select="Residence/FromDate"/>
									<xsl:text> — </xsl:text>
									<xsl:value-of select="Residence/ToDate"/>)<br/>
									Кімната: <xsl:value-of select="@room"/>
								</span>
							</td>

						</tr>
					</xsl:for-each>

				</table>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
