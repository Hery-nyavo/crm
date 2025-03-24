package site.easy.to.build.crm.util;

import com.opencsv.CSVReader;
import com.opencsv.CSVParserBuilder;
import com.opencsv.CSVReaderBuilder;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.IntStream;

@Service
public class ManageData {
    private final JdbcTemplate jdbcTemplate;

    public ManageData(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    public void importCSV(String filePath, String tableName) {
        try (FileReader fileReader = new FileReader(filePath)) {
            // Détection du séparateur
            char detectedSeparator = detectSeparator(filePath);
            System.out.println("Séparateur détecté : " + detectedSeparator);

            // Création d'un parser avec le bon séparateur
            CSVReader reader = new CSVReaderBuilder(fileReader)
                    .withCSVParser(new CSVParserBuilder().withSeparator(detectedSeparator).build())
                    .build();

            List<String[]> lignes = reader.readAll();
            if (lignes.isEmpty()) {
                System.out.println("Le fichier CSV est vide !");
                return;
            }

            // Lire la première ligne pour récupérer les noms de colonnes
            String[] columnNames = lignes.get(0);
            String columns = String.join(", ", columnNames);

            // Construire la requête SQL dynamique
            String placeholders = IntStream.range(0, columnNames.length)
                    .mapToObj(i -> "?")
                    .reduce((a, b) -> a + ", " + b)
                    .orElse("");

            String sql = "INSERT INTO " + tableName + " (" + columns + ") VALUES (" + placeholders + ")";

            // Préparer les données
            List<Object[]> batchArgs = new ArrayList<>();
            for (int i = 1; i < lignes.size(); i++) {
                batchArgs.add(lignes.get(i));
            }

            // Exécuter l'insertion en batch
            jdbcTemplate.batchUpdate(sql, batchArgs);

            System.out.println("Importation terminée !");
        } catch (IOException e) {
            System.out.println("Erreur de lecture du fichier CSV !");
            e.printStackTrace();
        } catch (Exception e) {
            System.out.println("Erreur SQL !");
            e.printStackTrace();
        }
    }

    /**
     * Détecte automatiquement le séparateur CSV en lisant la première ligne.
     */
    private char detectSeparator(String filePath) throws IOException {
        try (BufferedReader br = new BufferedReader(new FileReader(filePath))) {
            String firstLine = br.readLine();
            if (firstLine != null) {
                if (firstLine.contains(";")) {
                    return ';';
                } else if (firstLine.contains(",")) {
                    return ',';
                }
            }
        }
        return ','; // Par défaut, utilise la virgule
    }


}
