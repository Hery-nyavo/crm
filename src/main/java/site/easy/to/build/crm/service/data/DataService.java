package site.easy.to.build.crm.service.data;

import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;

public interface DataService {
    public void importCsv(MultipartFile file) throws IOException;
    public void reset() throws Exception;
    public void restore(MultipartFile file) throws Exception;
}
